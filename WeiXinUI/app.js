//app.js
App({
  onLaunch: function () {
    // 展示本地存储能力
    var logs = wx.getStorageSync('logs') || []
    logs.unshift(Date.now())
    wx.setStorageSync('logs', logs)
    let _this = this;
    // 获取用户信息
    wx.getSetting({
      success: res => {
        if (res.authSetting['scope.userInfo']) {
          // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
          wx.getUserInfo({
            success: res => {
              // 可以将 res 发送给后台解码出 unionId
              this.globalData.userInfo = res.userInfo
              

              // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
              // 所以此处加入 callback 以防止这种情况
              if (this.userInfoReadyCallback) {
                this.userInfoReadyCallback(res)
              }
            }
          })
        }
      }
    })
    
    // 登录
    wx.login({
      success: res => {
        if (res.code) {
          var getcode = res.code;
          wx.getUserInfo({
            success: function (res) {
              wx.request({
                url: 'http://localhost:8113/api/Values/Login',
                method:"get",
                data: {
                  code: getcode,
                  userName: res.userInfo.nickName
                },
                success: function (res) {
                  //把返回的openID写入全局数据
                  _this.globalData.userOpenId = res.data.OpenId

                  var set = wx.setStorage({
                    key: 'token',
                    data: res.data.session_key,
                    success: function (res) {

                    },
                    fail: function (res) { },
                    complete: function (res) { },
                  })
                  console.log(res)
                }
              })
            }})
          
        }
        // 发送 res.code 到后台换取 openId, sessionKey, unionId
      }
    })
  
  },
  globalData: {
    userInfo: null
  }
})