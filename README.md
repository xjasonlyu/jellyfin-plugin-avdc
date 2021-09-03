# Metadata Provider Plugin for Jellyfin/Emby

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/xjasonlyu/jellyfin-plugin-avdc/Build%20Plugin)
![GitHub](https://img.shields.io/github/license/xjasonlyu/jellyfin-plugin-avdc)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/xjasonlyu/jellyfin-plugin-avdc)

English | [简体中文](README_ZH.md)

## Features

- Movie Metadata
  - Title
  - Intro
  - Genre
  - Director
  - Actors
  - Studio
  - Tag/Series
  - Poster/Cover
- Movie Search
- Actor Metadata
  - Avatar
  - Birthday
  - Stature
  - Nation
  - Blood Type
  - Measurement
- Actor Search
- Scheduled Task
  - Auto Metadata Organization
  - Auto Update Plugin (Emby only)
- Fast Scraping Speed
- Cover **Auto Face-Recognition**

## Installation

**Jellyfin >= 10.7.0** || **Emby >= 4.6.0**

### Plugin

- Jellyfin Dashboard
    - Plugins -> Repositories
    - Add URL：[manifest.json](https://raw.githubusercontent.com/xjasonlyu/jellyfin-plugin-avdc/main/manifest.json)
    - Catalog -> AVDC，Install latest
    - Restart Jellyfin
    - Make sure AVDC is checked

- Emby Plugin Folder
    - Download [Releases](https://github.com/xjasonlyu/jellyfin-plugin-avdc/releases)
    - Extract dll file，upload to plugin folder
    - Restart Emby

### Configuration

- Backend API server build up
    - [Local Installation](https://github.com/xjasonlyu/avdc-api/wiki/%E6%9C%AC%E5%9C%B0%E5%AE%89%E8%A3%85)
    - [Docker Installation](https://github.com/xjasonlyu/avdc-api/wiki/Docker%E5%AE%89%E8%A3%85%EF%BC%88%E6%8E%A8%E8%8D%90%EF%BC%89)
    - [Synology Installation](https://github.com/xjasonlyu/avdc-api/wiki/Synology-%E7%BE%A4%E6%99%96%E5%AE%89%E8%A3%85%EF%BC%88%E5%8D%81%E5%88%86%E6%8E%A8%E8%8D%90%EF%BC%89)
- Enter URL and Token in config page
    - e.g. `http://192.168.1.2:5000`

## Naming

### Rules

1. Filename is Case-Insensitive
2. Most replaceable for `_` & `-`
3. Chinese subtitle with `-C` suffix
4. More Details at [Movie Naming](https://support.emby.media/support/solutions/articles/44001159102-movie-naming)

## Credits

- [jellyfin-plugin-douban](https://github.com/Libitum/jellyfin-plugin-douban): `MIT`
- [jellyfin-plugin-anime](https://github.com/jellyfin-archive/jellyfin-plugin-anime): `GPL-2.0`
- [jellyfin-plugin-template](https://github.com/jellyfin/jellyfin-plugin-template): `GPL-3.0`
- [Jellyfin.Plugin.PhoenixAdult](https://github.com/DirtyRacer1337/Jellyfin.Plugin.PhoenixAdult): `GPL-2.0`
- [Emby.Plugins.JavScraper](https://github.com/JavScraper/Emby.Plugins.JavScraper): `No License`
