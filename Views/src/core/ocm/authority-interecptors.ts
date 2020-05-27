import * as Promise from 'bluebird';
import Cookie from 'js-cookie'

/**
 * 装载ocm拦截器
 * @param axios axios库
 * @param opt ocm拦截器配置参数
 */
export function useInterecptor(axios, opt?:any){
    //request拦截
    axios.interceptors.request.use(config=>{
        let pathName = location.pathname;
        let token = Cookie.get('token');
        if(token && pathName != '/'){
            config.headers.common['token'] = token;
        }
        return config;
    });

    //response
    axios.interceptors.response.use(response=>{
        return response;
    }, err=>{
        if(err.response){
            switch(err.response.status){
                case 401:
                    Cookie.remove('token');
                    alert('未授权');
                    break;
            }
        }
        return Promise.reject(err.response);
    });
}