import * as Promise from 'bluebird';
import {_} from '../../lodash';
import {AccessMgr} from './';


export class ResourceMgr{
    public changeEvents:Array<any>;
    private list:Array<any>;
    private axios:any;
    private listUrl:string = '/api/Resource/GetAllResources';
    private saveUrl:string = '/api/Resource/Save';
    private removeUrl:string = '/api/Resource/Remove';
    private mgr:AccessMgr;
    constructor(mgr, axios:any, opt?:any){
        this.list = [];
        this.mgr = mgr;
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
                    let entities = result.data.list;
                    _.forEach(entities, (e)=>{
                        this.list.push(new ResourceEntity(e));
                    });
                    this.fireChange();
                    resolve();
                }else{
                    reject();
                }
            });
        });
    }
    getResourceById(id){
        return _.find(this.list, (v)=>{
            return v.id == id;
        });
    }
    newResource(parentId?){
        let entity = new ResourceEntity();
        entity.parentId = parentId || "-1";
        this.list.push(entity);
        return entity;
    }
    saveResource(entity:ResourceEntity){
        //insert, update
        let url = this.saveUrl;
        return new Promise((resolve, reject)=>{
            this.axios.post(url, entity.toJSON()).then((result)=>{
                if(result.data.isSuccess){
                    entity.id = result.data.data;
                    entity.localId = null;
                    this.fireChange();
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
            this.axios.get(this.removeUrl + "?id=" + id).then((result)=>{
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
        let ls = this.list;
        let vmRoot = this.mgr.domainService.buildResTree(ls);
        return vmRoot;
    }
    fireChange(){
        let vmLs = this.getList();
        _.forEach(this.changeEvents, (fun)=>{
            fun(vmLs);
        });
    }
}

export class ResourceEntity{
    public id:string;
    public localId:string;
    public parentId:string;
    public name:string;
    public code:string; //code为鉴权惟一编号
    public url:string;
    public type:string;
    public enable:boolean;
    public config:string; //包含icon, parameters, descript etc.
    public dirty:boolean;
    public order:number;
    constructor(info?:any){
        if(info){
            this.id = info.id;
            this.parentId = info.pId;
            this.name = info.resourceName;
            this.code = info.resourceCode;
            this.url = info.url;
            this.type = info.resourceType;
            this.order = info.orderNum;
            this.enable = info.enable;
            this.config = info.config;
            this.dirty = false;
        }else{
            this.config = "";
            this.localId = _.uniqueId('T_');
            this.parentId = "-1";
            this.type = "1";
            this.enable = true;
            this.dirty = true;
            this.name = this.code = this.url = '';
            this.order = 0;
        }
    }
    setProp(key,value){
        if(this.hasOwnProperty(key) && this[key] != value){
            this[key] = value;
            (!this.dirty) && (this.dirty = true)
        }
    }
    toJSON(){
        return {
            id: this.id,
            pId: this.parentId,
            resourceName: this.name,
            resourceCode: this.code,
            resourceType: this.type,
            url: this.url,
            enable: this.enable,
            config: this.config,
            orderNum: this.order
        }
    }
}