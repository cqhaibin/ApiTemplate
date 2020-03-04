## 目标
由于搭建的只是后端开发模板，主需要满足基本要求即可。
* 框架结构简单
* 数据库支持：Mssql, Mysql
* 构架易于上手
* 支持良好的业务扩展
* 是一套基础开发模板
## 技术选型
* 开发语言：c#
* 运行时 .net Framework4.5
* IoC：Autofac、Autofac.WebApi2
* Dto：AutoMapper
* 序列化：Newtonsoft.Json
* Orm：FreeSql
* Api：Aspnet.WebApi
* 数据库：首先MSSQL、次先Mysql
* 缓存：基于List或Dictionary实现单机内存级缓存
## 架构分层
![image](http://image.b-nature.cn/blog/api-template-01.png)
* 数据库访问层     
    * Freesql实体定义，也就是我们常说的是数据库实体
    * 提供dbContext上下文的方式访问
    * 复杂的sql操作接口定义及实现，如：多表查询、存储过程执行等
* 业务层     
    * 业务层拆分两个项目，BAccurate只做业务领域实体、值对象、系统配置对象、系统Model对象、AppService的定义等定义；BAccurate.Implement领域业务的实现
    * BAccurate.Implement可以依赖BAccurate.Repository.Fresql层，实现领域实体的数据持久化
    * Feesql实体定义未放在BAccurate层（业务规范项目），是为了防止领域实体与数据库实体混淆
    * BAccurate项目可以被其他所有项目使用 
* 服务层   
    * AppService.Implement和Webapi我都划分在服务层，其中webapi是一个贫血项目，只是把AppService做到WebApi接口化
    * AppService的接口和Model定义，都放到了BAccurate（业务规范项目）中，方便后期对AppService的重写和扩展
* UI层    
前端开发项目
## 接口规范
* 接口以类resetful api规范返回
* 返回结果规则一致     
![image](http://image.b-nature.cn/blog/api-template-02.png)
* 接口名称前缀动词：Get：表示查询；Post：提交数据（添加/修改）；Delete：移除数据
* 接口命名规则：接口名前缀+业务名称；以大驼峰命名
* 接口要进行统一管理
## 部署规范
* IIS托管部署
* 前端静态资源，托管到wwwroot目录    
![image](http://image.b-nature.cn/blog/api-template-03.png)
