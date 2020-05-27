import * as Promise from 'bluebird';
import {_} from '../../lodash';
import { AccessMgr } from '.';

export class UserMgr{    
    public changeEvents:Array<any>;
    private list:Array<any>;
    public axios:any;
    private listUrl:string = '/api/user/GetAll';
    private saveUrl:string = '/api/user/PostSave';
    private removeUrl:string = '/api/user/DeleteUser';
    private mgr:AccessMgr;
    constructor(mgr:AccessMgr, axios:any, opt?:any){
        this.mgr = mgr;
        this.list = [];
        this.changeEvents = [];
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
                        this.list.push(new UserEntity(this, e));
                    });
                    this.fireChange();
                    resolve();
                }else{
                    reject();
                }
            });
        });
    }
    newUser(){
        let entity = new UserEntity(this);
        this.list.push(entity);
        return entity;
    }
    saveUser(entity:UserEntity){
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
            this.axios.delete(this.removeUrl+"?id=" + id).then((result)=>{
                if(result.data.isSuccess){
                    this.fireChange();
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
    fireChange(){
        _.forEach(this.changeEvents, (fun)=>{
            fun(this.list);
        });
    }
}

export class UserEntity{
    private _getUrl = '/api/user/get';
    public id:number;
    public localId:string;
    public name:string = '';
    public realName:string = '';
    public enable:boolean = true;
    public config:string = '';
    public roles:Array<any>;
    private userMgr:UserMgr;
    public dirty:boolean;

    constructor(userMgr:UserMgr, obj?:any){
        this.userMgr = userMgr;
        this.roles = [];
        if(obj){
            this.id = obj.id;
            this.name = obj.userName;
            this.realName = obj.realName;
            this.enable = obj.enable;
            this.config = obj.config;
            this.dirty = false;
        }else{ 
            this.localId = _.uniqueId("T_");
            this.dirty = true;
        }
        if(window.webConfig.ocmApiUrl){
            this._getUrl = window.webConfig.ocmApiUrl + this._getUrl;
        }
    }
    setProp(key,value){
        if(this.hasOwnProperty(key) && this[key] != value){
            this[key] = value;
            (!this.dirty) && (this.dirty = true)
        }
    }
    load(){
        return new Promise((resolve, reject)=>{
            this.userMgr.axios.get(this._getUrl + '?userId=' + this.id).then((result)=>{
                if(result.data.isSuccess){
                    this.roles = result.data.data.roles;
                    resolve(result.data.data);
                }else{
                    reject();
                }
            });
        });
    }
    addRole(roleInfo){
        this.roles.push(roleInfo);
    }
    removeRole(id){
        _.remove(this.roles, (v)=>{
            return v.id == id;
        });
    }
    toJSON(){
        let ids = [];
        _.forEach(this.roles,(v)=>{
            ids.push(v.id);
        });
        return {
            id: this.id,
            userName: this.name,
            realName: this.realName,
            config: this.config,
            enable: this.enable,
            roles: ids
        };
    }
}