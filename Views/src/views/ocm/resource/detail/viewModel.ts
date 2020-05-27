import { ResourceEntity, ResourceMgr } from "../../../../core";

export class ViewModel{
    private entity:ResourceEntity;
    public codeOpt:any;
    public nameOpt:any;
    public urlOpt:any;
    public configOpt:any;
    public typeOpt:any;
    public enableOpt:any;
    public orderOpt:any;
    public parentInfo:KnockoutObservable<string>;
    public showDelete:KnockoutObservable<boolean>;
    public isEntityNull: KnockoutObservable<boolean>;
    _resourceMgr:ResourceMgr;
    _$root:any;
    _cur:any;
    
    constructor(params, componentDef, $root){
        this._$root = $root;
        this._resourceMgr = params.resourceMgr;
        this.parentInfo = ko.observable();
        this.showDelete = ko.observable(false);
        this.isEntityNull = ko.observable(true);
        this._cur = params.cur;
        this.codeOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('code', nVal);
            }
        };
        this.nameOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('name', nVal);
            }
        };
        this.urlOpt = {
            value: ko.observable(),
            onChange: (nVal)=>{
                this.setProp('url', nVal);
            }
        };
        this.orderOpt = {
            value: ko.observable(),
            onChange: (nVal)=>{
                this.setProp('order', nVal);
            }
        }

        this.configOpt = {
            multiline: true,
            height: 200,
            width: 400,
            value: ko.observable(),
            onChange: (nVal)=>{
                this.setProp("config", nVal);
            }
        };
        this.typeOpt = {
            panelHeight: 120,
            value: ko.observable(1),
            data: [{
                text: '接口',
                value: 1
            },{
                text: '菜单',
                value: 2
            }],
            onChange:(nVal)=>{
                this.setProp('type', nVal);
            }
        };
        this.enableOpt = {
            panelHeight: 120,
            value: ko.observable(0),
            data: [{
                text: '启用',
                value: 0
            },{
                text: '禁用',
                value: 1
            }],
            onChange:(nVal)=>{
                this.setProp('enable', nVal == 0 ? true : false);
            }
        };
        params.cur.subscribe((nVal)=>{
            this.entity = nVal;
            if(!this.entity){
                this.parentInfo('');
                this.codeOpt.value('');
                this.nameOpt.value('');
                this.urlOpt.value('');
                this.configOpt.value('');
                this.typeOpt.value(1);
                this.enableOpt.value(0);
                this.orderOpt.value(0);
                this.showDelete(false);
                this.isEntityNull(true);
                return;
            }
            this.codeOpt.value(this.entity.code);
            this.nameOpt.value(this.entity.name);
            this.urlOpt.value(this.entity.url);
            this.typeOpt.value(this.entity.type);
            this.configOpt.value(this.entity.config);
            this.enableOpt.value(this.entity.enable ? 0 : 1);
            this.orderOpt.value(this.entity.order);
            this.isEntityNull(false);
            let parentEntity:ResourceEntity = this._resourceMgr.getResourceById(this.entity.parentId);
            if(parentEntity){
                this.parentInfo(parentEntity.name + ";" + parentEntity.id);
            }
            if(this.entity.localId){
                this.showDelete(true);
            }else{
                this.showDelete(false);
            }
        });

    }
    onApply(){
        if(this.entity){
            this._resourceMgr.saveResource(this.entity).then((result)=>{
                this._$root.getMessager().alert('提示', '保存成功');
            }, (err)=>{
                this._$root.getMessager().alert('获取', '保存失败');
                console.log(err);
            });
        }
    }
    onDelete(){
        if(this.entity && this.entity.localId){ 
            this._resourceMgr.removeLocal(this.entity.localId);
            this._cur(null);
        }
    }
    setProp(key, value){
        this.entity && this.entity.setProp(key, value);
    }
}