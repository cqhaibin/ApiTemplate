const Webpack = require('webpack');
let WebpackWebServer = require('webpack-dev-server');
let options = require('./webpack.config.dev');
let prodOptions = require('./webpack.config.prod');
let env = process.env.NODE_ENV;

if(env == 'dev'){
    //dev
    var compile = Webpack(options);
    app = new WebpackWebServer(compile, options.devServer);
    app.listen(options.devServer.port);
    console.log( options.devServer.port + ' dev start ........'); 
}else {
    var compile = Webpack(prodOptions); 
    compile.run(function(err,stat){
        console.log('completed' + err);
    });
}