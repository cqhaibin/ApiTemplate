import {LocWorkInfoMgr, LocWorkInfoEntity} from '../../core';
import * as axios from 'axios';

export class ViewModel{
    public dlistOpt:any;
    _$root:any;
    _$rootDom:any;
    _$list:any;
    mgr:LocWorkInfoMgr;
    cur:KnockoutObservable<LocWorkInfoEntity>;
    constructor(params, componentDef, $root){
        this._$root = $root;
        this.mgr =new LocWorkInfoMgr(axios, null);
        this.cur = ko.observable();
        this._$rootDom = $(componentDef.element);
        this.dlistOpt = {
            data: [],
            textField: 'workName',
            valueField: 'id',
            onSelect: (index, row)=>{
                if(ko.unwrap(this.cur) && ko.unwrap(this.cur).localId){
                    //remove local
                    this.mgr.removeLocal(ko.unwrap(this.cur).localId);
                }
                this.cur(row);
            },
            toolbar:[
                {
                    text: '添加',
                    handler: ()=>{
                        let entity = this.mgr.newEntity();
                        this.cur(entity);
                    }
                }
            ]
        };
    }
    afterMount(){
        this._$list = this._$root.getInstanceByDom(this._$rootDom.find('.list')[0]);
        this.mgr.changeEvents.push((data)=>{
            this._$list.loadData(data);
            this.cur(null);
        });
        this.mgr.load();
    }
}