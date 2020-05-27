import { UserMgr, UserEntity } from "../../../../core";
import {_} from '../../../../core/lodash'

export class ViewModel{
    private entity:UserEntity;
    public rNameOpt:any;
    public nameOpt:any;
    public cfgOpt:any;
    public dlistOpt:any;
    public enableOpt:any;
    public showDelete:KnockoutObservable<boolean>;
    public isEntityNull: KnockoutObservable<boolean>;
    _userMgr:UserMgr;
    _$root:any;
    _$rootDom:any;
    _$list:any;
    _roles:any;
    _cur:any;
    
    constructor(params, componentDef, $root){
        this._$root = $root;
        this._userMgr = params.userMgr;
        this.showDelete = ko.observable(false);
        this._$rootDom = $(componentDef.element);
        this.isEntityNull = ko.observable(true);
        this._cur = params.cur;
        this._roles = params.roles;
        this.rNameOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('realName', nVal);
            }
        };
        this.nameOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('name', nVal);
            }
        };
        this.cfgOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('config', nVal);
            }
        };
        this.dlistOpt = {
            data: this._roles,
            checkbox: true,
            selectOnCheck: false,
            textField: 'roleName',
            valueField: 'id'
        }
        
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
                this.cfgOpt.value('');
                this.nameOpt.value('');
                this.rNameOpt.value('');
                this.enableOpt.value(0);
                this.showDelete(false);
                this.isEntityNull(true);
                this.setSelection(null);
                return;
            }
            this.setSelection(nVal);
            this.rNameOpt.value(this.entity.realName);
            this.nameOpt.value(this.entity.name);
            this.cfgOpt.value(this.entity.config);
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
            let rows = this._$list.getRows();
            _.map(nVal.roles, (r)=>{
                let row = _.find(rows, (or)=>{
                    return or.id == r.id;
                });
                let rIndex = this._$list.getRowIndex(row);
                this._$list.checkRow(rIndex);
            });
        }
    }
    clearSelectioNode(){
        let nodes = this._$list.getChecked();
        _.map(nodes, (n)=>{
            let rIndex = this._$list.getRowIndex(n);
            this._$list.uncheckRow(rIndex);
        });
    }

    afterMount(){
        this._$list = this._$root.getInstanceByDom(this._$rootDom.find('.role-list')[0]);
    }
    onApply(){
        if(this.entity){
            let nodes = this._$list.getChecked();
            if(nodes){
                let roles = [];
                _.map(nodes, (n)=>{
                    let _entity = _.cloneDeep(n);
                    roles.push(_entity);
                });
                this.entity.roles = roles;
            }
            this._userMgr.saveUser(this.entity).then((result)=>{
                this._$root.getMessager().alert('提示', '保存成功');
            }, (err)=>{
                this._$root.getMessager().alert('获取', '保存失败');
                console.log(err);
            });
        }
    }
    onDelete(){
        if(this.entity && this.entity.localId){
            this._userMgr.removeLocal(this.entity.localId);
            this._cur(null);
        } 
    }
    onRemoteDelete(){
        if(!this.entity){
            return;
        }
        let id = this.entity.id;
        if(!!id){
            this._$root.getMessager().confirm('提示', '您确认要删除此用户吗', (r)=>{
                if(r){
                    this._userMgr.remove(id).then((result)=>{
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