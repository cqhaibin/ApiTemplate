import * as Promise from 'bluebird';
import {_} from '../lodash';

/**
 * LocWorkInfo
 */
export class LocWorkInfoMgr{
    public changeEvents:Array<any>;
    private list:Array<any>;
    public axios:any;
    private listUrl:string = '/api/LocWorkInfo/GetAll';
    private saveUrl:string = '/api/LocWorkInfo/PostSave';
    private removeUrl:string = '/api/LocWorkInfo/Remove';
    constructor(axios:any, opt?:any){
        this.list = [];
        this.changeEvents = [];
        this.axios = axios;
        if(opt){
            this.listUrl = opt.listUrl;
            this.saveUrl = opt.saveUrl;
            this.removeUrl = opt.removeUrl;
        }
        if(window.webConfig.locApiUrl){
            this.listUrl = window.webConfig.locApiUrl + this.listUrl;
            this.saveUrl = window.webConfig.locApiUrl + this.saveUrl;
            this.removeUrl = window.webConfig.locApiUrl + this.removeUrl;
        }
    }
    load(){
        return new Promise( (resolve, reject)=>{
            this.axios.get(this.listUrl).then((result)=>{
                if(result.data.isSuccess){
                    this.list = [];
                    let entities = result.data.list;
                    _.forEach(entities, (e)=>{
                        this.list.push(new LocWorkInfoEntity(this, e));
                    });
                    this.fireChange();
                    resolve();
                }else{
                    reject();
                }
            });
        });
    }
    newEntity(){
        let entity = new LocWorkInfoEntity(this);
        this.list.push(entity);
        return entity;
    }
    save(entity:LocWorkInfoEntity){
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

export class LocWorkInfoEntity{    
    public localId:string;
    public createTime = '';
public id = 0;
public isUse = '';
public orderNum = 0;
public workName = '';
public workType = 0;

    public dirty:boolean;
	public _mgr;
    constructor(mgr,  obj?:any){
        this._mgr = mgr;
        //根据json解析对象
        if(obj){ 
			this.createTime=obj.createTime;
this.id=obj.id;
this.isUse=obj.isUse;
this.orderNum=obj.orderNum;
this.workName=obj.workName;
this.workType=obj.workType;

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
        //todo: load something
    }
    toJSON(){
        return {
            createTime:this.createTime,
id:this.id,
isUse:this.isUse,
orderNum:this.orderNum,
workName:this.workName,
workType:this.workType,

        };
    }
}
