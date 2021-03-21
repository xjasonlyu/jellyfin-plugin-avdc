# AVDC Plugin for Jellyfin

~~Jellyfin里的小姐姐总得有个身份~~

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/xjasonlyu/jellyfin-plugin-avdc/Build%20Plugin)
![GitHub](https://img.shields.io/github/license/xjasonlyu/jellyfin-plugin-avdc)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/xjasonlyu/jellyfin-plugin-avdc)

----------

## 特性支持

- 影片元数据刮削
- 影片元数据搜索
- 十分详细的信息
- 女优头像自动匹配
- 女优头像搜索替换
- 封面**自动人脸识别**
- 超级快的刮削速度

## 效果预览

🈚️🍑🈚️🍑🈚️🍑🈚️🍑🈚️🍑🈚️🍑

## Quickstart

Jellyfin >= 10.7.0

### 插件安装

- 进入Jellyfin控制台，Plugins -> Repositories
- 添加URL：[manifest.json](https://raw.githubusercontent.com/xjasonlyu/jellyfin-plugin-avdc/main/manifest.json)
- 在Catalog下找到AVDC，安装最新版
- 重启Jellyfin（⚠️）

### 配置使用

> 因为AVDC服务端会存储影片元数据和封面，所以为了丝滑的体验
> 暂不提供公益服，需要的可以自建。

- 开始刮削之前，请先确保`AVDC`后端服务器正确安装，详见这篇[README](https://github.com/xjasonlyu/AVDC/blob/main/README.md)
- 在插件配置页面下填入搭建的`AVDC`的服务器地址
- 填入服务端验证Token（如果需要的话）

### Docker

推荐使用Docker，可以免去配置`AVDC`服务器的麻烦

```yaml
version: '2.4'

services:
  avdc:
    image: ghcr.io/xjasonlyu/avdc:latest
    environment:
      - HTTP_PROXY=http://192.168.1.1:1080
      - HTTPS_PROXY=http://192.168.1.1:1080
    ports:
      - 5000:5000
    volumes:
      - ./avdc.db:/avdc.db
    networks:
      tunnel:
        aliases:
          - avdc.internal
    restart: unless-stopped
    container_name: avdc

  jellyfin:
    image: ghcr.io/linuxserver/jellyfin:latest
    logging:
      driver: none
    devices:
      - /dev/dri:/dev/dri
    volumes:
      - ...
    networks:
      - tunnel
    restart: unless-stopped
    container_name: jellyfin

networks:
  tunnel:
    driver: bridge
```

根据需要更改docker-compose.yml的参数，然后直接up

```text
docker-compose up -d
```

## 文件命名

### 规则

1. 字母可以忽略大小写
2. `_`和`-`可以互换
3. 确保番号在[Providers](#Providers)里可以搜到

### 例如

- `ABP-233.mp4`
- `ABP-233-C.mp4`
- `ABP-233-C-cd1.mp4`

## Providers

- [AVSOX](https://tellme.pw/avsox)
- [Jav321](https://www.jav321.com/)
- [JavBus](https://www.javbus.com/)
- [JavDB](https://javdb.com/)
- [~~JavLibrary~~](http://www.javlibrary.com/)
- [MsgTage](https://www.mgstage.com/)
- [Fanza](https://www.dmm.co.jp/)
- [Dlsite](https://www.dlsite.com/)
- [FC2](https://adult.contents.fc2.com/)
- [Xcity](https://xcity.jp/main/)

## 感谢

- [AV_Data_Capture](https://github.com/yoshiko2/AV_Data_Capture)
