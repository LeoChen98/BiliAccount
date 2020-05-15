# BiliAccount
B站账号操作封装

[![Version](https://img.shields.io/github/release/LeoChen98/BiliAccount.svg?label=Version)](https://github.com/LeoChen98/BiliAccount/releases)
[![GitHub issues](https://img.shields.io/github/issues/LeoChen98/BiliAccount.svg)](https://github.com/LeoChen98/BiliAccount/issues)
[![需要帮助的 issue](https://img.shields.io/github/issues/LeoChen98/BiliAccount/help%20wanted.svg?label=需要帮助的%20issue)](https://github.com/LeoChen98/BiliAccount/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22)
[![Language](https://img.shields.io/badge/%E8%AF%AD%E8%A8%80-%E4%B8%AD%E6%96%87-brightgreen.svg)](#)
[![DevLanguage](https://img.shields.io/badge/%E5%BC%80%E5%8F%91%E8%AF%AD%E8%A8%80-C%23-brightgreen.svg)](#)
[![Pull Request Welcome](https://img.shields.io/badge/Pull%20request-welcome-brightgreen.svg)](#)
[![GitHub license](https://img.shields.io/github/license/LeoChen98/BiliAccount.svg)](https://github.com/LeoChen98/BiliAccount/blob/master/LICENSE)

## 支持与依赖
框架|版本|依赖|备注
---|---|---|---
.net framework|≥3.5|（无）|`2.0.0.7`前只支持`.net framework 4.5`
.net standard|≥2.0|[Newtonsoft.Json](//github.com/JamesNK/Newtonsoft.Json) (≥ 12.0.3)<br/>[QRCoder](//github.com/codebude/QRCoder/) (≥ 1.3.6)<br/>[System.Drawing.Common](//github.com/dotnet/corefx) (≥ 4.7.0)|`2.0.0.7`起支持
.net core|≥3.0|[Newtonsoft.Json](//github.com/JamesNK/Newtonsoft.Json) (≥ 12.0.3)<br/>[QRCoder](//github.com/codebude/QRCoder/) (≥ 1.3.6)<br/>[System.Drawing.Common](//github.com/dotnet/corefx) (≥ 4.7.0)|`2.0.2.9`起支持

## 项目结构
项目名|备注
--|--
[BiliAccount](//github.com/LeoChen98/BiliAccount/wiki/BiliAccount)|BiliAccount类库
[BiliAccount.Geetest](//github.com/LeoChen98/BiliAccount.Geetest)|用于处理B站账号操作过程中的极验验证码的类库
[BiliAccount.Geetest.Controls](//github.com/LeoChen98/BiliAccount.Geetest.Controls)|用于处理B站账号操作过程中的极验验证码的控件和窗体类库
[BiliAccount.TestProject](//github.com/LeoChen98/BiliAccount/wiki/BiliAccount.TestProject)|测试工程
[BiliAccount.TestProject.Winforms](//github.com/LeoChen98/BiliAccount/wiki/BiliAccount.TestProject.Winforms)|Winform测试工程
[BiliAccount.TestProject.WPF](//github.com/LeoChen98/BiliAccount/wiki/BiliAccount.TestProject.WPF)|WPF测试工程
[BiliAccount.Core31TestProject](//github.com/LeoChen98/BiliAccount/wiki/BiliAccount.Core31TestProject)|Core3.1命令行测试工程

## 获取与使用

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BiliAccount?color=%23004080&logo=nuget)](https://www.nuget.org/packages/BiliAccount/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/LeoChen98/BiliAccount?include_prereleases&logo=github)](https://github.com/LeoChen98/BiliAccount/releases/latest)

**在使用以下命令时请将`Version`节点改为上述最新版本，否则可能会有错误。**

工具|命令/代码
--|--
Package Manager|`Install-Package BiliAccount -Version 2.2.0.12`
.NET CLI|`dotnet add package BiliAccount --version 2.2.0.12`
PackageReference|`<PackageReference Include="BiliAccount" Version="2.2.0.12" />`
Packet CLI|`paket add BiliAccount --version 2.2.0.12`

具体使用方法请见[Wiki](https://github.com/LeoChen98/BiliAccount/wiki)

## 开放源代码许可
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FLeoChen98%2FBiliAccount.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FLeoChen98%2FBiliAccount?ref=badge_large)
