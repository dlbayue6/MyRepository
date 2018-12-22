// pages/houseDetails/houseDetails.js
var Util = require('../../utils/util.js');
var check = require('../../utils/check.js');
var QQMapWX = require('../../utils/qqmap-wx-jssdk.min.js');
var qqmapsdk;
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    banner: {
      imgUrls: [],
      indicatorDots: true,
      autoplay: true,
      interval: 3000,
      duration: 200
    },
    shopList: {},
    id: '',
    maskShow: false,
    dataType: 1,
    userInfo: {
      name: '',
      tel: '',
      area: ''
    }
  },
  showMask: function (e) {
    this.setData({
      dataType: e.target.dataset.type,
      maskShow: true
    })
  },
  maskOpen: function () {
    this.setData({
      maskShow: false
    })
  },
  submit: function () {
    var _this = this;
    if (this.data.dataType == 1) {
      if (
        check.validTools('isnul', _this.data.userInfo.name, '请输入姓名') &&
        check.validTools('mobile', _this.data.userInfo.tel, '请输入正确的手机号码') &&
        check.validTools('house', _this.data.userInfo.area, '请输入正确的装修面积(1-500 m²)')
      ) {
        wx.showLoading({ title: '加载中', mask: true })
        wx.request({
          url: 'https://m.youzhu.com/centershop/externalSave',
          header: { 'content-type': 'application/x-www-form-urlencoded' },
          method: 'post',
          data: {
            username: _this.data.userInfo.name,
            telephone: _this.data.userInfo.tel,
            area: _this.data.userInfo.area,
            mark: 1
          },
          success: function (data) {
            wx.hideLoading()
            if (data.data.errorNo >= 0) {
              wx.showToast({ title: '成功' })
              _this.setData({
                maskShow: false,
                'userInfo.name': '',
                'userInfo.tel': '',
                'userInfo.area': ''
              })
            } else {
              wx.showModal({ title: '提示', content: data.data.message })
            }
          }
        })
      }
    } else {
      if (
        check.validTools('mobile', _this.data.userInfo.tel, '请输入正确的手机号码')
      ) {
        wx.showLoading({ title: '加载中', mask: true })
        wx.request({
          url: 'https://m.youzhu.com/centershop/sendAddress',
          header: { 'content-type': 'application/x-www-form-urlencoded' },
          method: 'post',
          data: {
            id: _this.data.shopList.id,
            telephone: _this.data.userInfo.tel
          },
          success: function (data) {
            wx.hideLoading()
            if (data.data.errorNo >= 0) {
              wx.showToast({ title: '成功' })
              _this.setData({
                maskShow: false,
                'userInfo.name': '',
                'userInfo.tel': '',
                'userInfo.area': ''
              })
            } else {
              wx.showModal({ title: '提示', content: data.data.message })
            }
          }
        })
      }
    }
  },
  changeName: function (e) {
    this.setData({ 'userInfo.name': e.detail.value })
  },
  changeTel: function (e) {
    this.setData({ 'userInfo.tel': e.detail.value })
  },
  changeArea: function (e) {
    this.setData({ 'userInfo.area': e.detail.value })
  },
  //加载房子图片
 /* loadBanner: function () {
    var _this = this;
    wx.request({
      url: 'https://m.youzhu.com/centershop/getCenterShopById',
      header: { 'content-type': 'application/x-www-form-urlencoded' },
      method: 'post',
      data: { id: _this.data.id },
      success: function (res) {
        wx.hideLoading()
        if (res.data.errorNo >= 0) {
          _this.setData({
            'banner.imgUrls': _this.img(res.data.data[0].wap_img),
            shopList: res.data.data[0]
          })
        }
      }
    })
  },*/
  /*添加收藏*/
  concern: function () {
    var that = this;
    wx.getStorage({
      key: 'token',
      success: function (res) {
    wx.request({
      url: 'http://localhost:8113/api/Values/AddConcern',
      method: 'post',
      data: {
        UserOpenId: app.globalData.userOpenId,
        HouseId: that.data.recommendHouse.Id
      },
      header: {
        'content-type': 'application/json',
        'Authorization': 'BasicAuth ' + res.data
      },
      success: function (res) {
       
      if(res.data==1)
      {
        wx.showToast({

          title: '收藏成功',

          icon: 'success',

          duration: 2000//持续时间，还可用超市函数退出，如下

        })

  
      }
       else if(res.data==2){
        wx.showToast({

          title: '已收藏',

          icon: 'fail',

          duration: 20000

        })

        setTimeout(function () {

          wx.hideToast()

        }, 2000)
       } 
       else{
        wx.showToast({

          title: '收藏失败',

          icon: 'fail',

          duration: 2000

        })
       }
      }
    })/**注意括号，有时不报错 */
      }
  })

  },
  callTel: function () {
    var _this = this;
    wx.makePhoneCall({
      phoneNumber: _this.data.recommendHouse.House_OwnerTel
    })
  },
  openMap: function () {
    var _this = this;
    qqmapsdk.geocoder({
      address: _this.data.recommendHouse.HouseLocation + _this.data.recommendHouse.House_Address,
      success: function (res) {
        wx.openLocation({
          latitude: res.result.location.lat,
          longitude: res.result.location.lng,
          scale: 10,
          name: _this.data.recommendHouse.House_Address,
          address: _this.data.recommendHouse.HouseLocation + _this.data.recommendHouse.House_Address
        })
      }
    });
  },
  img: function (src) {
    if (src) {
      if (src.indexOf(',') == '-1') {
        return [src];
      } else {
        src = src.split(',');
        return src;
      }
    }
  },
  /**
   * 生命周期函数--监听页面加载
   * option.参数，可以取到跳转路径传过来的值
   */
  onLoad: function (e) {
    wx.setNavigationBarTitle({
      title: '推荐美房'
    })

    if (e.id) {
      this.setData({
        id: e.id
      })
    }
    else{
      this.setData({
        id: 0
      })
    }
    qqmapsdk = new QQMapWX({
      key: 'UC3BZ-WENWG-75FQ7-IG5Z7-TVWX7-LHBXS'
    });
    //wx.showLoading({ title: '加载中', mask: true })
   // this.loadBanner()

    var that = this;
    wx.getStorage({
      key: 'token',
      success: function (res) {
        var pages = getCurrentPages();//集合中第一个为首页，最后一个为当前页
        var fistPage = pages[0];//首页
        var currPage = pages[pages.length - 1];   //当前页面————最后一个
        var prevPage = pages[pages.length - 2];  //上一个页面

        //直接调用上一个页面的setData()方法，把数据存到上一个页面中去
        /*获取目标房源*/
        wx.request({
          url: 'http://localhost:8113/api/Values/SelectHouseList',
          method: 'get',
          data: { 
            houseRentMoney: fistPage.data.houseRentMoney == null ? 1 : fistPage.data.houseRentMoney,
            habitableRoom: fistPage.data.habitableRoom == null ? "subian" : fistPage.data.habitableRoom,
            houseLocation: fistPage.data.houseLocation == null ? "suibian" : fistPage.data.houseLocation,
            houseId: that.data.id
          },
          header: {
            'content-type': 'application/json',
            'Authorization': 'BasicAuth ' + res.data
          },
          success: function (res) {
            console.log(res.data)/*接收的数据要 .data才能使用，即为返回的数据*/
            that.setData({
              houseList: res.data.list,
              recommendHouse: res.data.recommendHouse,
              'banner.imgUrls': that.img(res.data.recommendHouse.ImageUrls),
              //'banner.imgUrls':"",
              //shopList: res.data.data[0]
            })
          }
        })/**注意括号，有时不报错 */

      },
    })
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})