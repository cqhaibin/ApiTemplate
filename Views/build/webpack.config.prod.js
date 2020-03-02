let merge = require('webpack-merge');
const common = require('./webpack.config.common');
let webpack = require('webpack');

module.exports = merge(common,{
    mode: "production",
    devtool: 'none', //eval-source-map, none
    plugins:[
        new webpack.LoaderOptionsPlugin({
            options: {
                context: process.cwd() // or the same value as `context`
            }
        }),
        new webpack.DefinePlugin({
            DEBUG: false
        })
    ],
    optimization:{
        namedModules: true,
        splitChunks:{
            cacheGroups:{
                common:{
                    name: 'common',
                    chunks: 'all',
                    minChunks:2
                }
            }
        },
        runtimeChunk: true
    }
});