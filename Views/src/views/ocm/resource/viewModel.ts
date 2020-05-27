import {AccessMgr, ResourceEntity} from '../../../core';
import * as axios from 'axios';

export class ViewModel{
    public treeOpt:any;
    _$root:any;
    _$rootDom:any;
    _$tree:any;
    _$rMenu:any;
    doc:AccessMgr;
    cur:KnockoutObservable<ResourceEntity>;
    constructor(params, componentDef, $root){
        this._$root = $root;
        this.doc =new AccessMgr(axios, null);
        this.cur = ko.observable();
        this._$rootDom = $(componentDef.element);
        this.treeOpt = {
            data: [],
            onContextMenu:(e,node)=>{
                e.preventDefault();
                this._$tree.select(node.target);
                this._$rMenu.show({
                    left: e.pageX,
                    top: e.pageY
                });
            },
            onSelect: (node)=>{
                let item = this._$tree.getSelected();
                if(ko.unwrap(this.cur) && ko.unwrap(this.cur).localId){
                    //remove local
                    this.doc.resourceMgr.removeLocal(ko.unwrap(this.cur).localId);
                }
                this.cur(item.attributes.entity);
            }
        };
    }
    afterMount(){
        this._$tree = this._$root.getInstanceByDom(this._$rootDom.find('.resource-tree')[0]);
        this._$rMenu = this._$root.getInstanceByDom(this._$rootDom.find('.resource-rMenu')[0]);
        this._$rMenu.appendItem({
            text: '新建',
            onclick: ()=>{
                let parentId = this._$tree.getSelected();
                this.cur(this.doc.resourceMgr.newResource(parentId.id));
            }
        });
        this._$rMenu.appendItem({
            text: '删除',
            onclick: ()=>{
                let item = this._$tree.getSelected();
                let childs = this._$tree.getChildren(item.target);
                if(childs.length == 0){
                    this._$root.getMessager().confirm('提示', '您确认要删除此资源吗', (r)=>{
                        if(r){
                            this.doc.resourceMgr.remove(item.id).then((result)=>{
                                this._$root.getMessager().alert('提示', '删除成功');
                                this.cur(null);
                            }, (err)=>{
                                this._$root.getMessager().alert('警告', '删除失败');
                            });
                        }
                    });
                }
            }
        });
        this.doc.resourceMgr.changeEvents.push((data)=>{
            this._$tree.loadData(data);
            this.cur(null);
        });
        this.doc.resourceMgr.load();
    }
}