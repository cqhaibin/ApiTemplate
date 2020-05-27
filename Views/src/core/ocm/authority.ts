import * as Promise from 'bluebird';
import * as Cookie from 'js-cookie'

/**
 * 鉴权，登录，谁
 */
export class Authority{
    private loginUrl = '/api/Auth/PostLogin';
    private logoutUrl = '/api/Auth/LoginOut';
    private menuUrl = '/api/auth/GetMenus';
    private axios:any;
    constructor(axios, opt?:any){
        if(opt){
            this.loginUrl = opt.loginUrl;
            this.logoutUrl = opt.logoutUrl;
        }
        if(window.webConfig.ocmApiUrl){
            this.loginUrl = window.webConfig.ocmApiUrl + this.loginUrl;
            this.logoutUrl = window.webConfig.ocmApiUrl + this.logoutUrl;
        }
        this.axios = axios;
    }

    /**
     * 登录
     */
    login(info:LoginInfo){
        return new Promise((resolve, rejcet)=>{
            this.axios.post(this.loginUrl, info.toJSON()).then((result)=>{
                let data = result.data;
                if(data.isSuccess){
                    let token = data.data;
                    if(data.data && data.data.token){
                        token = data.data.token;
                    }
                    Cookie.set('token', token);
                    resolve(data);
                }else{
                    rejcet(data);
                }
            });
        });
    }

    /**
     * 获取菜单列表
     */
    getMenus(){
        return new Promise((resolve, reject)=>{
            this.axios.get(this.menuUrl).then((result)=>{
                if(result.data.isSuccess){
                    resolve(result.data);
                }else{
                    reject(result);
                }
            });
        });
    }

    /**
     * 修改密码
     * 修改密码后，必然会登出
     */
    changePwd(param:ChangePasswordParam){ 
        this.logout();
    }

    /**
     * 登出
     */
    logout(){
        this.axios.get(this.logoutUrl);
        Cookie.remove('token');
    }
}

export class LoginInfo{
    public name:string;
    public password:string;
    constructor(name:string, password:string){
        this.name = name;
        this.password = password;
    }
    toJSON(){
        return {
            name: this.name,
            pwd: this.password
        }
    }
}

export class ChangePasswordParam{
    public newPwd:string;
    public userId:string;
}