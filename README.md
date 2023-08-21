<div align="center">
  <img src="https://github.com/SaltyFishFly/FluentCloudMusic/blob/master/Assets/StoreLogo.scale-400.png"/>
</div>
<h1 align="center">FluentNetease</h1>

一个第三方的网易云音乐客户端,基于UWP和WinUI
## 如何获取？

首先，确保你的Windows版本大于等于Windows 11 build 22000。Windows 10由于排版问题不受支持。

### 方法一：
直接在[Microsoft Store下载](https://www.microsoft.com/store/apps/9MSRXWZK6T4X)

### 方法二：
在[Github Releases页面](https://github.com/SaltyFishFly/FluentCloudMusic/releases)下载。需要打开 设置->隐私和安全性->开发者选项。

### 方法三：
使用Visual Studio构建。

需要以下组件：
- Windows 11 SDK (Version10.0.22000)
- 通用Windows平台开发

在Visual Studio内打开此仓库，右键“FluentCloudMusic(Universal Windows)”并选择“发布”，使用自签名证书编译，并安装输出的.msix包。

## 关于:
此软件使用[MIT License](https://mit-license.org/)开源

使用了以下开源项目:
- [Binaryify/NeteaseCloudMusicApi](https://github.com/Binaryify/NeteaseCloudMusicApi)

- [wwh1004/NeteaseCloudMusicApi](https://github.com/wwh1004/NeteaseCloudMusicApi)

- [Newtonsoft.Json](https://www.newtonsoft.com/json)
