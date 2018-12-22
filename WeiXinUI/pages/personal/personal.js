// pages/personal/personal.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    showsearch: false,   //显示搜索按钮
    searchtext: '',  //搜索文字
    servicelistall:[],
    servicelist: [], //服务集市列表
    scrolltop: null, //滚动位置
    page: 0 , //分页

    dataType:1
  },


  
  /**
  
  * 生命周期函数--监听页面加载
  
  */
  fetchServiceData: function (houses) {  //获取我的发布列表
    let _this = this;
  
    this.setData({
      servicelist: houses
    })

  },
  inputSearch: function (e) {  //输入搜索文字
    this.setData({
      showsearch: e.detail.cursor > 0,
      searchtext: e.detail.value
    })
  },
  submitSearch: function () {  //提交搜索
    console.log(this.data.searchtext);
    
  },
  scrollHandle: function (e) { //滚动事件
    this.setData({
      scrolltop: e.detail.scrollTop
    })
  },
  goToTop: function () { //回到顶部
    this.setData({
      scrolltop: 0
    })
  },
  scrollLoading: function () { //滚动加载
  
    this.allHouses();
  },
  onPullDownRefresh: function () { //下拉刷新
    this.setData({
      page: 0,
      servicelist: []
    })
   
    this.allHouses();
    setTimeout(() => {
      wx.stopPullDownRefresh()
    }, 1000)
  },
  onLoad: function (options) {
    this.setData({

      currentData: options.checkIndex,
      dataType: options.roleId,
      concernState:1
    });
    app.globalData.roleId = this.data.dataType;
//******应该是判断滑块下标，进行选择加载************* */
     var that = this;
     wx.getStorage({
      key: 'token',
      success: function (res) {
       //获取我收藏的房屋
        that.myConcren();
       //房源管理的所有房源
        that.allHouses();
       //我发布的房源
        that.myHouses();
       //获取我审批任务列表
        that.getApplicationList()
      },
    });
  },
  approvalAction: function (e) {
    var _this = this;
    wx.request({
      url: 'http://localhost:8113/api/House/ApprovalAction',
      method: 'get',
      data: {
        id: e.currentTarget.id
      },
      header: {
        'content-type': 'application/json',

      },
      success: function (res) {

        if (res.data == 1) {
        
          wx.showToast({

            title: '完成审批',

            icon: 'success',

            duration: 2000//持续时间，还可用超市函数退出，如下

          })


        }
      }
    })/**注意括号，有时不报错 */

  },
  getApplicationList: function (res) {
    //获取我审批任务列表
    var that = this;
    wx.request({
      url: 'http://localhost:8113/api/Application/GetApprovalActionList',
      method: 'get',
      data: {
        roleId: that.data.dataType
      },
      header: {
        'content-type': 'application/json',
        
      },
      success: function (res) {
        console.log(res.data)/*接收的数据要 .data才能使用，即为返回的数据*/
        that.setData({
          applicationList: res.data,

        })
      }
    })/**注意括号，有时不报错 */

  },
  myConcren:function(res)
  {
         //获取我收藏的房屋
    var that = this;
         wx.request({
           url: 'http://localhost:8113/api/Values/ConcernHouse',
           method: 'get',
           data: {
             userOpenId: app.globalData.userOpenId
           },
           header: {
             'content-type': 'application/json',
            
           },
           success: function (res) {
             console.log(res.data)/*接收的数据要 .data才能使用，即为返回的数据*/
             that.setData({
               houseList: res.data,

             })
           }
         })/**注意括号，有时不报错 */

   },

  //跳转到发布房源
  Publish() {
    wx.navigateTo({
      url: '/pages/publishHouse/publishHouse?roleId=' + this.data.dataType,
    })
  },

  //取消关注
  cancelConcern:function(e)
  {
   var _this=this;
    wx.request({
      url: 'http://localhost:8113/api/Values/cancelConcern',
      method: 'get',
      data: {
        houseId: e.currentTarget.id
      },
      header: {
        'content-type': 'application/json',
       
      },
      success: function (res) {

        if (res.data == 1) {
          _this.setData({
            concernState:0
          })
          wx.showToast({

            title: '已取消',

            icon: 'success',

            duration: 2000//持续时间，还可用超市函数退出，如下

          })


        }
      }
    })/**注意括号，有时不报错 */

  },
  //我的发布房屋
  myHouses:function()
  {
    var _this = this;
    wx.request({
      url: 'http://localhost:8113/api/Values/SelectMyHouses',
      method: 'get',
      data: {
        userOpenId2: app.globalData.userOpenId
      },
      header: {
        'content-type': 'application/json',

      },
      success: function (res) {
       
        _this.fetchServiceData(res.data);

      }
    })/**注意括号，有时不报错 */

  },
  //发布的所有房屋
  allHouses: function () {
    var _this = this;
    wx.request({
      url: 'http://localhost:8113/api/Values/SelectHouses',
      method: 'get',
      
      header: {
        'content-type': 'application/json',

      },
      success: function (res) {
        console.log(res.data)
        _this.setData({
          servicelistall:res.data
        })

      }
    })/**注意括号，有时不报错 */

  },
  //获取当前滑块的index

  bindchange: function (e) {

    const that = this;

    that.setData({

      currentData: e.detail.current

    })
    app.globalData.currentData = e.detail.current;
  },

  //点击切换，滑块index赋值

  checkCurrent: function (e) {

    const that = this;



    if (that.data.currentData === e.target.dataset.current) {

      return false;

    } else {
      if (e.target.dataset.current == 0) {
        that.myConcren();
      }
      else if (e.target.dataset.current==1)
      {
        that.myHouses(); 
      }
      else if (e.target.dataset.current == 4) {
        that.getApplicationList();
      }
     else  if (e.target.dataset.current == 5) {
        that.allHouses();
      }
      
      that.setData({

        currentData: e.target.dataset.current

      })
      app.globalData.currentData = e.target.dataset.current;
    }

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