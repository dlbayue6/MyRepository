//index.js
//获取应用实例
const app = getApp()
Page({
  onShareAppMessage: function () {
    return {
      title: '乐租松江',
      desc: '乐租松江'
    }
  },
 
  onLoad() {
    let that = this
    //调用应用实例的方法获取全局数据
    app.getUserInfo(function (userInfo) {
      //更新数据
      that.setData({
        userInfo: userInfo
      });
    })
  },
  bindPickerChange(e) {
    let _this = this;
    _this.setData({
      houseTypeIndex: e.detail.value
    })
    //_this.data.inputContent[e.currentTarget.id] = e.detail.value; //，把页面标签的id属性，设为 自定义变量的属性，写入数据
  },
  findHouse(e) {
    let _this = this;
    let _datas = 0;
    let _types = _this.data.houseType[_this.data.houseTypeIndex];
    let _salary = _this.data.inputContent.currentSalary;
    let _Location = _this.data.inputContent.workPalce;
    // wx.navigateTo({
    //   url: '../logs/logs?type=1'
    // });
    // return false;

    if (typeof (_salary) == 'undefined' || !_salary) {
      wx.showModal({
        title: '提示',
        content: '请输入目标租金',
        showCancel: false
      });
      return false;
    }
    this.setData({ houseRentMoney: _salary, habitableRoom: _types, houseLocation: _Location })

    

    wx.navigateTo({
      url: '../houseDetails/houseDetails'//?houseRentMoney=1&habitableRoom_ID=2&house_Address="sda" 
    });
  },
  bindchangeInput(e) {
    this.data.inputContent[e.currentTarget.id] = e.detail.value;
  },
toPersonal:function()
{
  wx.navigateTo({
    url: '../houseDetails/houseDetails'//?houseRentMoney=1&habitableRoom_ID=2&house_Address="sda" 
  });
},
 data: {
    userInfo: {},
    imgbx: {
      topImg: '/images/p1-top.png',
      titImg: '/images/p1-tit.png',
      bomImg: '/images/p1-bom.png'
    },
    button: {
      defaultSize: 'default',
      disabled: false,
      plain: false,
      loading: false
    },
   houseType: ["一室一厅", "两室一厅", "三室一厅", "五室两厅", "其他"],
    houseTypeIndex: 0,
    inputContent: {},

    showModal: true,
   showModalRole: false,

    motto: 'Hello World',
    userInfo: {},
    hasUserInfo: false,
    canIUse: wx.canIUse('button.open-type.getUserInfo')
  },
  toShowModal(e) {
    this.setData({
      showModal: true

    })
  },

  hideModal() {
    this.setData({
      showModal: false
    });
  },
  toShowModalRole(e) {
    //后续：登陆后返回角色，把角色id写入全局数据，判断id，除超级管理员，以下不执行，直接跳转个人中心并传递id和滑块下标
    this.setData({
      showModalRole: true

    })
  },
  hideModalRole() {
    this.setData({
      showModalRole: false
    });
  },
  //事件处理函数
  bindViewTap: function() {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
 onLoad: function () {
    if (app.globalData.userInfo) {
      this.setData({
        userInfo: app.globalData.userInfo,
        hasUserInfo: true
      })
    } else if (this.data.canIUse){
      // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
      // 所以此处加入 callback 以防止这种情况
      app.userInfoReadyCallback = res => {
        this.setData({
          userInfo: res.userInfo,
          hasUserInfo: true
        })
      }
    } else {
      // 在没有 open-type=getUserInfo 版本的兼容处理
      wx.getUserInfo({
        success: res => {
          app.globalData.userInfo = res.userInfo
          this.setData({
            userInfo: res.userInfo,
            hasUserInfo: true
          })
        }
      })
    }
  },
  getUserInfo: function(e) {
    console.log(e)
    app.globalData.userInfo = e.detail.userInfo
    this.setData({
      userInfo: e.detail.userInfo,
      hasUserInfo: true
    })
  }
})
