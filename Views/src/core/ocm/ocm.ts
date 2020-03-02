import * as Promise from 'bluebird';

export class Ocm {
  _prefix;
  _axios;
  constructor (prefix, axios) {
    this._prefix = prefix || '/ocm'
    this._axios = axios
  }
  getMenu (resourceCode) {
    let that = this
    let query = ''
    if (resourceCode) {
      query = '?resCode=' + resourceCode
    }
    return new Promise((resolve, reject) => {
      that._axios.get(that._prefix + '/api/auth/GetIdentity' + query).then((result) => {
        if (result.data.isSuccess) {
          let list = result.data.data.menus
          if (list) {
            list = list[0].childs
          }
          resolve({
            menus: list,
            user: result.data.data.user
          })
        } else {
          reject(result)
        }
      }, (err)=>{
        reject(err)
      })
    })
  }
  /**
  * 登出
  */
  logout () {
    return new Promise((resolve, reject) => {
      //@ts-ignore
      $.removeCookie('token')
      this._axios.get(this._prefix + '/api/Auth/LoginOut').then((result) => {
        if (result.data.isSuccess) {
          resolve(result.data)
        } else {
          reject(result)
        }
      })
    })
  }
   /**
   * 自动登录
   */
  login(userInfo){
    debugger
    return new Promise((resolve, rejcet) => {
        this._axios.post(this._prefix + '/api/Auth/PostLogin', userInfo).then((result)=>{
            let data = result.data;
            if(data.isSuccess){
                //@ts-ignore
                $.cookie('token', data.data);
                resolve(data);
            }else{
                rejcet(data);
            }
        });
    });
}
}
