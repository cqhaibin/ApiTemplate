import * as Promise from 'bluebird';

/**
 * 装载ocm拦截器
 * @param axios axios库
 * @param opt ocm拦截器配置参数
 */
export function useInterecptor (axios, opt) {
  // request拦截
  axios.interceptors.request.use(config => {
    let pathName = location.pathname
    //@ts-ignore
    let token = $.cookie('token')
    if (token && pathName !== '/') {
      config.headers.common['token'] = token
    }
    return config
  })

  // response
  axios.interceptors.response.use(response => {
    return response
  }, err => {
    if (err.response) {
      switch (err.response.status) {
        case 401:
          //@ts-ignore
          $.removeCookie('token')
          alert('未授权')
          break
      }
    }
    return Promise.reject(err.response)
  })
}
