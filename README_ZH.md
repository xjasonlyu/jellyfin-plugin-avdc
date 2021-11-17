# Metadata Provider Plugin for Jellyfin/Emby

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/xjasonlyu/jellyfin-plugin-avdc/Build%20Plugin)
![GitHub](https://img.shields.io/github/license/xjasonlyu/jellyfin-plugin-avdc)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/xjasonlyu/jellyfin-plugin-avdc)

[English](README.md) | 简体中文

----------

## 功能

- 影片元数据刮削
  - 标题
  - 简介
  - 类型
  - 导演
  - 演员
  - 工作室
  - 标签/系列
  - 海报/封面
- 影片元数据搜索
- 演员元数据刮削
  - 头像
  - 生日
  - 身高
  - 罩杯
  - 三围
  - 血型
  - 国籍
- 演员元数据搜索
- 定时计划任务
  - 自动整理数据
  - 自动更新插件（Emby only）
- 封面**自动人脸识别**
- 超级快的刮削速度

## 安装

**Jellyfin >= 10.7.0** || **Emby >= 4.6.0**

### 插件

- Jellyfin控制台
  - Plugins -> Repositories
  - 添加URL：[manifest.json](https://raw.githubusercontent.com/xjasonlyu/jellyfin-plugin-avdc/main/manifest.json)
  - 在Catalog下找到AVDC，安装最新版
  - 重启Jellyfin（⚠️）
  - 在媒体库设置中勾选AVDC（☑️）

- Emby插件目录
  - 从[Releases](https://github.com/xjasonlyu/jellyfin-plugin-avdc/releases)下载插件
  - 解压出dll文件，上传到插件目录
  - 重启Emby（⚠️）

### 配置

- AVDC-API服务端搭建
  - [本地安装](https://github.com/xjasonlyu/avdc-api/wiki/%E6%9C%AC%E5%9C%B0%E5%AE%89%E8%A3%85)
  - [Docker安装（推荐）](https://github.com/xjasonlyu/avdc-api/wiki/Docker%E5%AE%89%E8%A3%85%EF%BC%88%E6%8E%A8%E8%8D%90%EF%BC%89)
  - [群晖安装（十分推荐）](https://github.com/xjasonlyu/avdc-api/wiki/Synology-%E7%BE%A4%E6%99%96%E5%AE%89%E8%A3%85%EF%BC%88%E5%8D%81%E5%88%86%E6%8E%A8%E8%8D%90%EF%BC%89)
- 在插件配置页面下填入搭建的服务端地址与Token：
  - 如：`http://192.168.1.2:5000`

## 文件命名

### 规则

1. 字母可以忽略大小写
2. `_`和`-`可以互换
3. 确保番号在[数据来源](#数据来源)里可以搜到
4. 中文字幕在文件名后加入`-C`后缀
5. 多集影片在文件名后加入如`-cd1`后缀
6. 其他奇怪命名均有可能导致无法自动识别

### 例子

- `ABP-725.mp4`
- `FC2-1734831.mp4`
- `FC2PPV-1734831.mp4`
- `422ION-0062-C.mp4`
- `012414_743-cd1.wmv`
- `012414_743-cd2.wmv`
- `259LUXU-1377-C-cd1.mkv`

## 数据来源

- [AVSOX](https://tellme.pw/avsox)
- [Jav321](https://www.jav321.com/)
- [JavBus](https://www.javbus.com/)
- [JavDB](https://javdb.com/)
- [~~JavLibrary~~](http://www.javlibrary.com/)
- [MGS](https://www.mgstage.com/)
- [Fanza](https://www.dmm.co.jp/)
- [Dlsite](https://www.dlsite.com/)
- [FC2](https://adult.contents.fc2.com/)
- [Xcity](https://xcity.jp/main/)

## 感谢

- [jellyfin-plugin-douban](https://github.com/Libitum/jellyfin-plugin-douban): `MIT`
- [jellyfin-plugin-anime](https://github.com/jellyfin-archive/jellyfin-plugin-anime): `GPL-2.0`
- [jellyfin-plugin-template](https://github.com/jellyfin/jellyfin-plugin-template): `GPL-3.0`
- [Jellyfin.Plugin.PhoenixAdult](https://github.com/DirtyRacer1337/Jellyfin.Plugin.PhoenixAdult): `GPL-2.0`
- [Emby.Plugins.JavScraper](https://github.com/JavScraper/Emby.Plugins.JavScraper): `No License`
