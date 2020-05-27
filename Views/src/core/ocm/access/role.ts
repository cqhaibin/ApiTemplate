import * as Promise from 'bluebird';
import {_} from '../../lodash';
import { AccessMgr } from '.';

/**
 * 用户角色
 */
export class RoleMgr{
    public roleChangeEvents:Array<any>;
    private list:Array<any>;
    public axios:any;
    private listUrl:string = '/api/role/GetAll';
    private saveUrl:string = '/api/role/PostSave';
    private removeUrl:string = '/api/role/DeleteRole';
    private mgr: AccessMgr;
    constructor(mgr, axios:any, opt?:any){
        this.mgr = mgr;
        this.list = [];
        this.roleChangeEvents = [];
        this.axios = axios;
        if(opt){
            this.listUrl = opt.listUrl;
            this.saveUrl = opt.saveUrl;
            this.removeUrl = opt.removeUrl;
        }
        if(window.webConfig.ocmApiUrl){
            this.listUrl = window.webConfig.ocmApiUrl + this.listUrl;
            this.saveUrl = window.webConfig.ocmApiUrl + this.saveUrl;
            this.removeUrl = window.webConfig.ocmApiUrl + this.removeUrl;
        }
    }
    load(){
        return new Promise( (resolve, reject)=>{
            this.axios.get(this.listUrl).then((result)=>{
                if(result.data.isSuccess){
                    this.list = [];
                    let entities = result.data.list;
                    _.forEach(entities, (e)=>{
                        this.list.push(new RoleEntity(this, e));
                    });
                    this.fireRoleChange();
                    resolve();
                }else{
                    reject();
                }
            });
        });
    }
    newRole(){
        let entity = new RoleEntity(this);
        this.list.push(entity);
        return entity;
    }
    saveRole(entity:RoleEntity){
        //insert, update
        let url = this.saveUrl;
        return new Promise((resolve, reject)=>{
            this.axios.post(url, entity.toJSON()).then((result)=>{
                if(result.data.isSuccess){
                    this.load();
                    resolve(result.data);
                }else{
                    reject(result)
                }
            });
        });
    }
    removeLocal(id){
        _.remove(this.list, (v) => {
            return v.localId == id;
        }); 
    }
    remove(id){
        _.remove(this.list, (v) => {
            return v.id == id;
        }); 
        return new Promise((resolve, reject)=>{
            this.axios.delete(this.removeUrl+'?id=' + id).then((result)=>{
                if(result.data.isSuccess){
                    this.fireRoleChange();
                    resolve(result.data);
                }else{
                    reject(result)
                }
            });
        });
    }
    getList(){
        return this.list;
    }
    fireRoleChange(){
        _.forEach(this.roleChangeEvents, (fun)=>{
            fun(this.list);
        });
    }
}

export class RoleEntity{    
    private getUrl:string = '/api/role/get';
    public id:number;
    public localId:string;
    public name:string = '';
    public code:string = '';
    public enable:boolean = true;
    public description:string = '';
    public dirty:boolean;
    public resources:Array<any>;
    private _role:RoleMgr;
    constructor(role,  obj?:any){
        this._role = role;
        if(role.opt){
            this.getUrl = role.opt.getUrl;
        }
        if(window.webConfig.ocmApiUrl){
            this.getUrl = window.webConfig.ocmApiUrl + this.getUrl;
        }
        this.resources = [];
        //根据json解析对象
        if(obj){ 
            this.id = obj.id;
            this.name = obj.roleName;
            this.code = obj.roleCode;
            this.enable = obj.enable;
        }else{
            this.localId = _.uniqueId('T_');
            this.dirty = true;
        }
    }
    setProp(key,value){
        if(this.hasOwnProperty(key) && this[key] != value){
            this[key] = value;
            (!this.dirty) && (this.dirty = true)
        }
    }
    load(){
        //todo: resource of model diff this.resource.
        return new Promise((resolve, reject)=>{
            this._role.axios(this.getUrl + "?roleId=" + this.id).then((result)=>{
                if(result.data.isSuccess){
                    this.resources = result.data.data.resources;
                    resolve(result.data.data);
                }else{
                    reject();
                }
            });
        });
    }
    addResource(resInfo){
        this.resources.push(resInfo);
    }
    removeResource(id){
        _.remove(this.resources, (v)=>{
            return v.id == id;
        });
    }
    toJSON(){
        let ids = [];
        _.forEach(this.resources,(v)=>{
            ids.push({
                id: v.id,
                status: v.status
            });
        });
        return {
            id: this.id,
            roleName: this.name,
            roleCode: this.code,
            enable: this.enable,
            description: this.description,
            resources: ids
        };
    }
}
