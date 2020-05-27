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
            '/ocm/*':{
                target: 'http://localhost:8002/', //代理的目标服务器
                changeOrigin: true,
                pathRewrite: {
                    "/ocm/": "/"
                },
                secure: false //是否需要ssl的验证
            }
        }
    }
});