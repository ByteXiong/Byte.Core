### Byte.Core
基础版 :joy:  后续会更新


### 配置
  Byte.Core.Common 独立的公共模块

  Byte.Core.SqlSugar 目前单库版,后面会更新 SqlSugar5.0

  Byte.Core.EntityFramework EF连接类(未完善)

## 目录结构
```shell

 |-- Byte.Core.Common 基础库
 |   |-- Attributes 特性
 |   |-- Cache 缓存
 |   |-- ClassLibrary
 |   |-- CodeTemplate 模版基础库
 |   |-- Consts
 |   |-- DbLogProvider
 |   |-- Enums 
 |   |-- Extensions 扩展
 |   |-- Filters 过滤器
 |   |-- Global
 |   |-- Helpers 帮助类
 |   |-- IoC  依赖注入

 |   |-- Middlewares
 |   |-- Models
 |   |-- Mvc
 |   |-- Quartz 定时器
 |   |-- Services
 |   |-- SnowflakeId
|	|-- SnowflakeIdHelper 雪花Id生成类
 |   |-- Web
 |-- Byte.Core.EntityFramework EF连接类(未完善)
 |   |-- Attributes
 |   |-- BusinessLogics
 |   |-- CodeGenerator 代码生成器
 |   |-- CodeTemplate 代码器模版
 |   |-- DbContextCore 数据库适配类
 |   |-- Extensions
 |   |-- IDbContext
 |   |-- Models
 |   |-- Options 数据库连接配置
 |   |-- Pager
 |   |-- Repository
 |-- Byte.Core.SqlSugar  SqlSugar5.0
 |   |-- Base   表基类
 |   |-- BaseBusinessLogic  BLL 基类
 |   |-- ConfigOptions  数据库连接配置
 |   |-- IDbContext 上下文
 |   |-- Model  
 |   |-- Pager 分页配置
 |   |-- Repository DAL 基类

```