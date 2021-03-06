import {AccessMgr, UserEntity} from '../../../core';
import * as axios from 'axios';

export class ViewModel{
    public dlistOpt:any;
    _$root:any;
    _$rootDom:any;
    _$list:any;
    doc:AccessMgr;
    cur:KnockoutObservable<UserEntity>;
    public roles:KnockoutObservable<any>;
    constructor(params, componentDef, $root){
        this._$root = $root;
        this.doc =new AccessMgr(axios, null);
        this.cur = ko.observable();
        this.roles = ko.observable();
        this._$rootDom = $(componentDef.element);
        this.dlistOpt = {
            data: [],
            textField: 'name',
            valueField: 'id',
            onSelect: (index, row)=>{
                if(ko.unwrap(this.cur) && ko.unwrap(this.cur).localId){
                    //remove local
                    this.doc.roleMgr.removeLocal(ko.unwrap(this.cur).localId);
                }
                (<UserEntity>row).load().then((result)=>{
                    this.cur(row);
                });
            },
            toolbar:[
                {
                    text: '添加',
                    handler: ()=>{
                        let entity = this.doc.userMgr.newUser();
                        this.cur(entity);
                    }
                }
            ]
        };
    }
    afterMount(){
        this._$list = this._$root.getInstanceByDom(this._$rootDom.find('.user-list')[0]);
        this.doc.userMgr.changeEvents.push((data)=>{
            this._$list.loadData(data);
            this.cur(null);
        });
        this.doc.domainService.getAllRoles().then((result)=>{
            this.roles(result);
            this.doc.userMgr.load();
        });
    }
}