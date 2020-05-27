var path = require('path'); 
let HtmlWebpackPlugin = require('html-webpack-plugin');
let CopyWebpackPlugin = require('copy-webpack-plugin');
let miniCssExtractPlugin = require('mini-css-extract-plugin');
let babelOptions = require('./babel.json');
let projectPath = path.join(__dirname, '..');

/**
 * 导出配置对象
 */
module.exports = {
    entry:{
        index: [ 'babel-polyfill',path.join(projectPath , 'src', 'index.ts')],
        ocm: [ path.join(projectPath , 'src', 'runtime.ts')]
    }, 
    resolve: {
        extensions: ['.ts', '.js', '.json']
    },
    output:{
        path: path.join(projectPath, 'dist')
    },
    module:{
        rules:[
            {
                test: /\.(ts)$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: babelOptions
                    },
                    {
                        loader: 'ts-loader'
                    }
                ]
            },
            {
                test: /\.(html)$/,
                loader: ['html-loader']
            },
            {
                test: /\.(ejs)$/,
                loader: ['ejs-loader']
            },
            {
                test: /\.(scss)$/,
                use: [miniCssExtractPlugin.loader, 'css-loader', 'sass-loader'],
                include: path.join(__dirname, '..', 'src'),
                exclude: /node_modules/
            },
            {
                test: /\.(css)$/,
                use: [miniCssExtractPlugin.loader, 'css-loader']
            },
            {
                test: /\.(png|jpg|gif)$/,
                use:[
                    {
                        loader: 'file-loader',
                        options:{
                            publicPath: '/images',
                            outputPath: path.join(projectPath, 'dist', 'images')
                        }
                    }
                ]
            }
        ]
    },
    externals:{
        "lodash": {
            root: "_"
        }
    },
    plugins:[
        new HtmlWebpackPlugin({
            title: '主页',
            template: 'template.ejs',
            filename: 'index.html',
            chunks: ['index']
        }),
        new CopyWebpackPlugin([{
            from: path.join(projectPath, 'static', '**/*.*'), 
            to: path.join(projectPath, 'dist')
        }]),
        new miniCssExtractPlugin({
            filename: '[name].css',
            chunkFileName: '[id].css'
        })
    ]
};