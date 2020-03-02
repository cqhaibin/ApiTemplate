import * as Promise from 'bluebird';
import {use as Views} from './views';
import axios from 'axios';
import { Ocm, useInterecptor } from './core';

 //@ts-ignore
 if(DEBUG){
    window.webConfig = {
        mcgsApiUrl: '/mcgs', //组态,gis相关接口地址
        ocmApiUrl: '/ocm',
        locApiUrl: '/loc'
    }
    //@ts-ignore
    /*window.loginInfo = {
        name: 'admin',
        pwd: 'admin'
    }*/
}
//注册easyui
window.koeasyui.use(ko);

Views();

useInterecptor(axios, null)
let ocm = new Ocm(window.webConfig.ocmApiUrl, axios)

//测试组件
let rootVm={
    $core: {
        ocm: ocm
    },
    $cfg:{
        links: null
    },
    getLink: function(id){
        const that = this;
        return new Promise((resolve) => {
            if (that.$cfg.links) {
                const url = that.$cfg.links[id];
                resolve(url);
                return;
            }
            axios.get('/static/cfg/links.json').then((result) => {
                that.$cfg.links = result.data;
                const url = that.$cfg.links[id];
                resolve(url);
            })
        })
    },
    /**
     * 获取全局的弹出窗口
     */
    getDialogs: function(){
        let $dialogs = window.koeasyui.getContextFor(document.getElementById('dialogs'));
        return $dialogs;
    },
    getInstance: function(id:string){
        let instance = window.koeasyui.getContextFor(document.getElementById(id));
        return instance;
    },
    getMessager(){
        return this.getInstance('messager');
    },
    
    getInstanceByDom(dom:Element){
        let instance = window.koeasyui.getContextFor(dom);
        return instance;
    }
}
ko.applyBindings(rootVm, document.getElementById('app'));