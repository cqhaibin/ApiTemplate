import {Authority, LoginInfo} from '../../../core';
import * as axios from 'axios';

export class ViewModel{
  public name:KnockoutObservable<string>;
  public pwd:KnockoutObservable<string>;
  _$root:any;
  auth:Authority
  constructor(params, componentDef, $root){
    this.auth = new Authority(axios);
    this._$root = $root;
    this.name = ko.observable();
    this.pwd = ko.observable();
  }
  autoLogin(vm, event){
    let keyCode = event.keyCode;
    if(keyCode==13){
      event.target.blur();
      this.login();
      return false;
    }
    return true;
  }
  login(){
    let name = ko.unwrap(this.name);
    let pwd = ko.unwrap(this.pwd);
    if(!name || !pwd){
      this._$root.getMessager().alert('提示', '用户名或密码不能为空');
      return;
    }
    this.auth.login(new LoginInfo(name, pwd)).then((result)=>{
      //todo: ok, redirect.
      let originUrl = window.location.href;
      let url = '/';
      if(originUrl.indexOf("?returl=") > 0){
        url = originUrl.substring(originUrl.indexOf("?returl=") + 8);
      }
      window.location.href = url;
    }, (error)=>{
      //failure
      this._$root.getMessager().alert('提示', '用户名或密码错误');
    });
  }
}