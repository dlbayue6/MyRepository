var app = getApp()
Page({
  data: {
    shopList: {},
    locationCity: ''
  },
  loadBanner: function () {
    var _this = this;
    wx.request({
      url: 'https://m.youzhu.com/centershop/CenterShopList',
      header: { 'content-type': 'application/x-www-form-urlencoded' },
      method: 'post',
      data: { city: _this.data.locationCity ? _this.data.locationCity : '全国' },
      success: function (res) {
        wx.hideLoading()
        wx.stopPullDownRefresh()
        if (res.data.errorNo >= 0) {
          _this.setData({
            shopList: res.data.data
          })
        }
      }
    })
  },
  getAll: function () {
    this.setData({
      locationCity: '全国'
    })
    wx.showLoading({ title: '加载中', mask: true })
    this.loadBanner()
  },
  onPullDownRefresh: function () {
    this.loadBanner();
  },
  onLoad: function () {
    var pages = getCurrentPages();//集合中第一个为首页，最后一个为当前页
    var fistPage = pages[0];//首页
    var currPage = pages[pages.length - 1];   //当前页面————最后一个
    var prevPage = pages[pages.length - 2];  //上一个页面
    this.setData({
      houseList:prevPage.data.houseList,
      locationCity: wx.getStorageSync('locationCity')
    })
    //wx.showLoading({ title: '加载中', mask: true })
    //this.loadBanner()
  }
})
