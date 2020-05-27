import { RoleEntity, RoleMgr } from "../../../../core";
import {_} from '../../../../core/lodash'

export class ViewModel{
    private entity:RoleEntity;
    public codeOpt:any;
    public nameOpt:any;
    public descOpt:any;
    public treeOpt:any;
    public enableOpt:any;
    public showDelete:KnockoutObservable<boolean>;
    public isEntityNull: KnockoutObservable<boolean>;
    _roleMgr:RoleMgr;
    _$root:any;
    _$rootDom:any;
    _$tree:any;
    _resTree:any;
    _cur:any;
    
    constructor(params, componentDef, $root){
        this._$root = $root;
        this._roleMgr = params.roleMgr;
        this.showDelete = ko.observable(false);
        this._$rootDom = $(componentDef.element);
        this.isEntityNull = ko.observable(true);
        this._cur = params.cur;
        this._resTree = params.resTree;
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
        this.descOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('description', nVal);
            }
        };
        this.treeOpt = {
            data: this._resTree,
            checkbox: true
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
                this.codeOpt.value('');
                this.nameOpt.value('');
                this.descOpt.value('');
                this.enableOpt.value(0);
                this.showDelete(false);
                this.isEntityNull(true);
                this.setSelection(null);
                return;
            }
            this.setSelection(nVal);
            this.codeOpt.value(this.entity.code);
            this.nameOpt.value(this.entity.name);
            this.descOpt.value(this.entity.description);
            this.enableOpt.value(this.entity.enable ? 0 : 1);
            this.isEntityNull(false);
            if(this.entity.localId){
                this.showDelete(true);
            }else{
                this.showDelete(false);
            }
        });
    }
    setSelection(nVal){
        this.clearSelectioNode();
        if(nVal){
            _.map(nVal.resources, (r)=>{
                if(r.status == 1){
                    let node = this._$tree.find(r.id);
                    this._$tree.check(node.target);
                }
            });
        }
    }
    clearSelectioNode(){
        let nodes = this._$tree.getChecked(['checked']);
        _.map(nodes, (n)=>{
            this._$tree.uncheck(n.target);
        });
    }

    afterMount(){
        this._$tree = this._$root.getInstanceByDom(this._$rootDom.find('.resource-tree')[0]);
    }
    onApply(){
        if(this.entity){
            let nodes = this._$tree.getChecked(['checked', 'indeterminate']);
            if(nodes){
                let res = [];
                _.map(nodes, (n)=>{
                    let _entity = _.cloneDeep(n.attributes.entity);
                    _entity.status = n.checkState == 'indeterminate' ? 2 : 1;
                    res.push(_entity);
                });
                this.entity.resources = res;
            }
            this._roleMgr.saveRole(this.entity).then((result)=>{
                this._$root.getMessager().alert('提示', '保存成功');
            }, (err)=>{
                this._$root.getMessager().alert('获取', '保存失败');
                console.log(err);
            });
        }
    }
    onDelete(){
        if(this.entity && this.entity.localId){
            this._roleMgr.removeLocal(this.entity.localId);
            this._cur(null);
        } 
    }
    onRemoteDelete(){
        if(!this.entity){
            return;
        }
        let id = this.entity.id;
        if(!!id){
            this._$root.getMessager().confirm('提示', '您确认要删除此角色吗', (r)=>{
                if(r){
                    this._roleMgr.remove(id).then((result)=>{
                        this._$root.getMessager().alert('提示', '删除成功');
                        this._cur(null);
                    }, (err)=>{
                        this._$root.getMessager().alert('警告', '删除失败');
                    });
                }
            });
        }
    }
    setProp(key, value){
        this.entity && this.entity.setProp(key, value);
    }
}