# BiliAccount
B站账号操作封装

## 支持与依赖
框架|版本|依赖
---|---|---
.net framework|≥3.5|（无）
.net standard|2.0|Newtonsoft.Json (>= 12.0.3)<br/>QRCoder (>= 1.3.6)<br/>System.Drawing.Common (>= 4.7.0)

## 项目结构
项目名|备注
--|--
[BiliAccount](https://github.com/LeoChen98/BiliAccount/wiki/BiliAccount)|BiliAccount类库
[BiliAccount.TestProject](https://github.com/LeoChen98/BiliAccount/wiki/BiliAccount.TestProject)|测试工程

## 获取与使用

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BiliAccount?color=%23004080&logo=nuget)](https://www.nuget.org/packages/BiliAccount/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/LeoChen98/BiliAccount?include_prereleases&logo=github)](https://github.com/LeoChen98/BiliAccount/releases/latest)

*在使用以下命令时请将`Version`节点改为上述最新版本，否则可能会有错误。

工具|命令/代码
--|--
Package Manager|`Install-Package BiliAccount -Version 2.0.0.7`
.NET CLI|`dotnet add package BiliAccount --version 2.0.0.7`
PackageReference|`<PackageReference Include="BiliAccount" Version="2.0.0.7" />`
Packet CLI|`paket add BiliAccount --version 2.0.0.7`

## 开放源代码许可
### Newtonsoft.Json 12.0.3
<https://www.newtonsoft.com/json>

Copyright (c) 2019 Newtonsoft

Licensed under MIT

### QRCoder 1.3.6
<https://github.com/codebude/QRCoder/>

Copyright © 2011 - 2018 Raffael Herrmann

Licensed under MIT

### System.Drawing.Common
<https://github.com/dotnet/corefx>

© Microsoft Corporation. All rights reserved.

Licensed under MIT
