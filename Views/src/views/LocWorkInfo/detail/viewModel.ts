import { LocWorkInfoMgr,LocWorkInfoEntity } from "../../../core";
import {_} from '../../../lodash'

export class ViewModel{
    private entity:LocWorkInfoEntity;
    public createTimeOpt:any;
public idOpt:any;
public isUseOpt:any;
public orderNumOpt:any;
public workNameOpt:any;
public workTypeOpt:any;

    public showDelete:KnockoutObservable<boolean>;
    public isEntityNull: KnockoutObservable<boolean>;
    _mgr:LocWorkInfoMgr;
    _$root:any;
    _$rootDom:any;
    _$list:any;
    _cur:any;
    
    constructor(params, componentDef, $root){
        this._$root = $root;
        this._mgr = params.mgr;
        this.showDelete = ko.observable(false);
        this._$rootDom = $(componentDef.element);
        this.isEntityNull = ko.observable(true);
        this._cur = params.cur;
        this.createTimeOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('createTime', nVal);
            }
        };
this.idOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('id', nVal);
            }
        };
this.isUseOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('isUse', nVal);
            }
        };
this.orderNumOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('orderNum', nVal);
            }
        };
this.workNameOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('workName', nVal);
            }
        };
this.workTypeOpt = {
            value: ko.observable(),
            onChange:(nVal)=>{
                this.setProp('workType', nVal);
            }
        };


        params.cur.subscribe((nVal)=>{
            this.entity = nVal;
            if(!this.entity){
                this.createTimeOpt.value();
this.idOpt.value(0);
this.isUseOpt.value();
this.orderNumOpt.value(0);
this.workNameOpt.value();
this.workTypeOpt.value(0);

                this.showDelete(false);
                this.isEntityNull(true);
                return;
            }
            this.createTimeOpt.value(this.entity.createTime);
this.idOpt.value(this.entity.id);
this.isUseOpt.value(this.entity.isUse);
this.orderNumOpt.value(this.entity.orderNum);
this.workNameOpt.value(this.entity.workName);
this.workTypeOpt.value(this.entity.workType);

            this.isEntityNull(false);
            if(this.entity.localId){
                this.showDelete(true);
            }else{
                this.showDelete(false);
            }
        });
    }
    onApply(){
        if(this.entity){
            this._mgr.save(this.entity).then((result)=>{
                this._$root.getMessager().alert('提示', '保存成功');
            }, (err)=>{
                this._$root.getMessager().alert('获取', '保存失败');
                console.log(err);
            });
        }
    }
    onDelete(){
        if(this.entity && this.entity.localId){
            this._mgr.removeLocal(this.entity.localId);
            this._cur(null);
        } 
    }
    onRemoteDelete(){
        if(!this.entity){
            return;
        }
        let id = this.entity.id;
        if(!!id){
            this._$root.getMessager().confirm('提示', '您确认要删除此吗', (r)=>{
                if(r){
                    this._mgr.remove(id).then((result)=>{
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