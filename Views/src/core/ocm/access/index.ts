import {ResourceEntity, ResourceMgr} from './resource';
import * as Promise from 'bluebird';
import {_} from '../../lodash';
import { RoleMgr } from './role';
import { UserMgr } from './user';

export class AccessMgr{
    resourceMgr:ResourceMgr;
    roleMgr:RoleMgr;
    userMgr:UserMgr;
    domainService:AccessDomainService;
    constructor(axios:any, opt?:any){
        this.domainService = new AccessDomainService(this, axios, opt);
        this.resourceMgr = new ResourceMgr(this, axios, opt);
        this.roleMgr = new RoleMgr(this, axios, opt);
        this.userMgr = new UserMgr(this, axios, opt);
    }
}
export class AccessDomainService{
    private listUrl:string = '/api/Resource/GetAllResources';
    private roleListUrl:string = '/api/role/GetAll';
    private reloadCache:string = '/api/Auth/Reload';
    private axios:any;
    constructor(mgr:AccessMgr, axios, opt?:any){
        this.axios = axios;
        if(opt){
            this.listUrl = opt.listUrl;
        }
        if(window.webConfig.ocmApiUrl){
            this.listUrl = window.webConfig.ocmApiUrl + this.listUrl;
            this.roleListUrl = window.webConfig.ocmApiUrl + this.roleListUrl;
            this.reloadCache = window.webConfig.ocmApiUrl + this.reloadCache;
        }
    }
    getAllRoles(){
        return new Promise((resolve, reject)=>{
            this.axios.get(this.roleListUrl).then((result)=>{
                if(result.data.isSuccess){
                    resolve(result.data.list);
                }else{
                    reject();
                }
            }, (err)=>{
                reject(err);
            });
        });
    }

    getResTree(){
        return new Promise((resolve, reject)=>{
            this.axios.get(this.listUrl).then((result)=>{
                if(result.data.isSuccess){
                    let entities = result.data.list;
                    let ls = [];
                    _.forEach(entities, (e)=>{
                        ls.push({
                            id: e.id,
                            name: e.resourceName,
                            parentId: e.pId
                        });
                    });
                    let root = this.buildResTree(ls);
                    resolve(root);
                }else{
                    reject();
                }
            });
        });
    }
    buildResTree(ls){
        let vmRoot
        let root = _.find(ls, (v)=>{
            return v.id == '-1';
        });
        if(!root){
            root = {};
            root.name = '资源根';
            root.id = "-1";
            root.parentId = null;
        }
        vmRoot = {
            id: root.id,
            text: root.name,
            attributes:{
                entity: root
            },
            children:[]
        }
        let first = _.filter(ls, (v)=>{
            return v.parentId && v.parentId == root.id;
        });
        this._buildResTree(first, vmRoot, ls);
        return [vmRoot];
    }
    _buildResTree(list, dest, ls){
        _.map(list, (v)=>{
            let item = {
                id: v.id,
                text: v.name,
                attributes: {
                    entity: v
                },
                children:[]
            };
            dest.children.push(item);
            let first = _.filter(ls, (vv)=>{
                //item.id is null, new node.
                return item.id && vv.parentId == item.id;
            });
            if(first && first.length > 0){
                this._buildResTree(first, item, ls);
            }
        });
    }
    reload(){
        return new Promise((resolve, reject)=>{
            this.axios.get(this.reloadCache).then((result)=>{
                if(result.data.isSuccess){
                    resolve();
                }else{
                    reject();
                }
            }, (err)=>{
                reject(err);
            });
        });
    }
}
export * from './resource';
export * from './user';
export * from './role';
