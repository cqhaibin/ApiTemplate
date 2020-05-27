import {AccessMgr, RoleEntity} from '../../../core';
import * as axios from 'axios';

export class ViewModel{
    _$root:any;
    doc:AccessMgr;
    reseting;
    constructor(params, componentDef, $root){
        this._$root = $root;
        this.doc =new AccessMgr(axios, null);
    }
    afterMount(){
        
    }
    reload(){
        if(this.reseting){
            return;
        }
        this.reseting = true;
        this.doc.domainService.reload().then(()=>{
            this.reseting = false;
            this._$root.getMessager().alert('提示', '服务重启成功');
        },()=>{
            this._$root.getMessager().alert('提示', '服务重启失败');
            this.reseting = false;
        });
    }
}