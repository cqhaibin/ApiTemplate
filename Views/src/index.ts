
import {use as Views} from './views';

 //@ts-ignore
 if(DEBUG){
    window.webConfig = {
        mcgsApiUrl: '/mcgs', //组态,gis相关接口地址
        ocmApiUrl: '/ocm',
        locApiUrl: '/loc'
    }
}
//注册easyui
window.koeasyui.use(ko);

Views();

//测试组件
let rootVm={
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