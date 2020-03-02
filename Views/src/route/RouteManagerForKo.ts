import { RouteInfo } from './routeInfo';
import * as page from 'page';

export class RouteMangerForKo{
    public page;
    public current;
    public routes:Array<RouteInfo>;
    public component:KnockoutObservable<any>;
    public baseUrl: string;
    public isSelected:boolean;

    constructor(isSelected:boolean, url?:string){
        this.page = page;
        this.component = ko.observable();
        this.routes = new Array<RouteInfo>();
        this.baseUrl = url || '';
        this.isSelected = isSelected;
    }

    init() {
        if(this.baseUrl.length > 0){
            this.page.base(this.baseUrl);
        }
        return this.component;
    }    
    run() {
        page({ 
            hashbang:true,
        });
    }
    getRoutesByGroupName(groupName: string) {
        return $.map(this.routes, (item, index)=>{
            if(item.group == groupName){
                return item;
            }
        });
    }
    addRoute(route:RouteInfo, parent?:RouteInfo) {
        let index = this.getIndex(route.route);
        if(parent){
            route.parent = parent;
            if(index != -1){
                this.routes[index].parent = parent;
            }
        }
        if(index == -1){
            route.href = this.baseUrl + route.route;
            if(this.isSelected){
                //@ts-ignore 
                route.selected = ko.observable(false);
            }
            this.routes.push(route);
            this.page(route.route, (cxt, next)=>{
                this.redirect(route, cxt);
            });
        }
    }

    redirect(route, cxt){
        this.clearSelectedRoute(route);
        if(route.selected){
            route.selected(true);
        }
        if(route.renderDom){
            let _component = this.getComponent(route.renderDom);
            if(_component){
                _component(this.createComponent(route, cxt));
            }else if(route.parent) {
                this.redirect(route.parent, cxt);
                let handler = window.setTimeout(()=>{
                    window.clearTimeout(handler);
                    _component = this.getComponent(route.renderDom);
                    if(_component){
                        _component(this.createComponent(route, cxt));
                    }
                },0);
            }
        }else{
            this.component(this.createComponent(route, cxt));
        }
    }
    getIndex(path){
        let existIndex = -1;
        $.map(this.routes, (item, index)=>{
            if(item.route == path){
                existIndex = index;
                return false;
            }
        });
        return existIndex;
    }
    getSingleRoute(path){
        let route = null;
        $.map(this.routes, (item, index)=>{
            if(item.route == path){
                route = item;
                return false;
            }
        });
        return route;
    }
    createComponent(route:RouteInfo, cxt:any){
        return {
            id: route.id,
            name: route.common,
            title: route.text,
            params: route.params,
            context: cxt
        }
    }
    clearSelectedRoute(origin:RouteInfo){
        if(!this.isSelected) return
        this.routes.map((item)=>{
            if(item.group == origin.group){
                //@ts-ignore
                item.selected(false);
            }
        });
    }
    getComponent(id:string){
        let viewModel = ko.contextFor(document.getElementById(id));
        if(viewModel && viewModel.$data){
            return viewModel.$data.component;
        }
        return null;
    }

    goto(url: any) {
        this.page(url);
    }
}