let path = require('path');
let merge = require('webpack-merge');
let webpack = require('webpack');

const common = require('./webpack.config.common');

module.exports = merge(common,{
    mode: 'development',
    devtool: 'source-map',
    plugins:[
        new webpack.DefinePlugin({
            DEBUG: true
        })
    ],
    devServer:{
        contentBase: './dist',
        port: 9001,
        proxy:{
            '/loc/*':{
                target: 'http://localhost:8090/', //代理的目标服务器
                // pathRewrite: {"^/api": ""}, //重写向地址，这里是去年/api前缀，如果没有，则/api/a访问的地址是：http://192.168.0.13:1991/api/a
                secure: false //是否需要ssl的验证
            },
            '/ocm/*':{
                target: 'http://192.168.2.212', //代理的目标服务器
                secure: false //是否需要ssl的验证
            },
            '/earth/*':{
                target: 'http://192.168.2.212', //代理的目标服务器
                secure: false //是否需要ssl的验证
            }
        }
    }
});