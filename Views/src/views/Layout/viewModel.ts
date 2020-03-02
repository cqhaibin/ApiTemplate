import {RouteInfo, RouteMangerForKo} from '../../route';
let routeObj = null;

export class ViewModel{
    public comp:KnockoutObservable<any>;
    private routeManager:RouteMangerForKo;
    public menus:any;
    public menusOpt:any;
    private selectRoute:string;
    private $root;
    private userInfo:KnockoutObservable<string>;
    private isLogin:KnockoutObservable<boolean>
    constructor(params, componentDef, $root){
        this.$root = $root;
        this.userInfo = ko.observable();
        this.isLogin = ko.observable(false);
        this.routeManager = new RouteMangerForKo(false, null);
        this.comp = this.routeManager.init();
        this.menus = [];
        this.menusOpt = {
            data: ko.observableArray(),
            multiple: false,
            onSelect: (item:any)=>{
                let url = item.attributes.info.route
                this.routeManager.goto(url);
            }
        }
    }
    afterMount(){
        let loadRouteFun = ()=>{
            //init route
            let url = window.location.href;
            this.selectRoute = url.substring(url.indexOf('#!')+2);
            let routeObjs = routeObj;
            this.registryRoute(routeObjs, this.menus);
            this.menusOpt.data(this.menus);
            this.routeManager.run();
        };
        let loadMenuFun = () =>{
            //ocm
            this.$root.$core.ocm.getMenu('OriSystem').then((result) => {
                routeObj = result.menus;
                this.userInfo(result.user.userName);
                this.isLogin(true);
                loadRouteFun();
            }, errMsg => {
                this.userInfo('未登录');
                this.isLogin(false);
            })
        }

        //@ts-ignore
        if(window.loginInfo){
            //@ts-ignore
            this.$root.$core.ocm.login(window.loginInfo).then((result)=>{
                loadMenuFun();
            });
        }else{
            loadMenuFun();
        }
    }
    onProfile(){
        if(ko.unwrap(this.isLogin)){
            this.$root.getMessager().confirm('警告', '您确认要退出系统吗', (r)=>{
                if(r){
                    this.logout();
                }
            })
        }else{
            this.gotoLogin();
        }
    }
    gotoLogin () {
        window.open('/ocm/wwwroot/login.html', '_self')
    }
    logout () {
        this.$root.$core.ocm.logout().then((result) => {
            window.open('/ocm/wwwroot/login.html', '_self')
        })
    }
    registryRoute(routeObjs, menus){
        $.each(routeObjs, (i, v)=>{
            let cfg = v.config ? JSON.parse(v.config) : {};
            let routeInfo = new RouteInfo(v.id, v.url, v.resourceName, v.group, cfg.common, v.renderDom, {
                url: cfg.url
            });
            if(v.url){
                this.routeManager.addRoute(routeInfo);
            }
            let m = {
                id: v.id,
                text: v.resourceName,
                iconCls: cfg.iconCls,
                attributes:{
                    info: routeInfo
                }
            };
            //selected
            if(this.selectRoute == routeInfo.route){
                this.menusOpt.selectedItemId = v.id;
                menus.state = 'open';
            }

            if(v.childs && v.childs.length >0){
                //@ts-ignore
                m.children = [];
                //@ts-ignore
                this.registryRoute(v.childs, m);
            }
            if($.isArray(menus)){
                menus.push(m);
            }else{
                menus.children.push(m);
            }
        });
    }
}