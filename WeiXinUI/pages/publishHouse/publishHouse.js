// pages/publishHouse/publishHouse.js
const app=getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    imageUrls:"",
    imageUrl:"",
    habitableRoom: [],
    habitableRoomindex: 0,
    orientations: [],
    orientationsindex: 0,
    uploadimgs: [], //上传图片列表
    editable: false //是否可编辑
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.setData({
      roleId:options.roleId
    })
    this.fetchData()
  },
  fetchData: function () {
    this.setData({
      habitableRoom: ["请选择", "一室一厅", "两室一厅", "三室一厅", "五室两厅",  "其他"],
 
      orientations: ["请选择", "南北通透", "东西", "东南", "东北", "朝东", "朝南", "朝西", "朝北", "西南", "西北",  "其他"]
    })
  },
  bindPickerChange: function (e) { //下拉选择
    const eindex = e.detail.value;
    const name = e.currentTarget.dataset.pickername;
    // this.setData(Object.assign({},this.data,{name:eindex}))
    switch (name) {
      case 'HabitableRoom':
        this.setData({
          habitableRoomindex: eindex
        })
        break;
      case 'Orientations':
        this.setData({
          orientationsindex: eindex
        })
        break;
 
      default:
        return
    }
  },
  //开始上传 选择图片
  chooseImage: function (e) {
    let _this = this;
    _this.setData({
      imageType : e.currentTarget.dataset.type
    })
   
    wx.showActionSheet({
      itemList: ['从相册中选择', '拍照'],
      itemColor: "#f7982a",
      success: function (res) {
        if (!res.cancel) {
          if (res.tapIndex == 0) {
            _this.chooseWxImage('album')
          } else if (res.tapIndex == 1) {
            _this.chooseWxImage('camera')
          }
        }
      }
    })
  },

  chooseWxImage: function (type) {
    let _this = this;

          if (_this.data.imageType == 1)
          {
            wx.chooseImage({
              //count: 3,  //最多可以选择的图片总数
              //sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
              sizeType: ['original', 'compressed'],
              sourceType: [type],
              success: function (res) {
                _this.setData({
                  uploadimgs: _this.data.uploadimgs.concat(res.tempFilePaths)
                })
              }
            })
         
          }

  
    if (_this.data.imageType == 2) {
      wx.chooseImage({
        count:1,  //最多可以选择的图片总数
        //sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
        sizeType: ['original', 'compressed'],
        sourceType: [type],
        success: function (res) {
          _this.setData({
            uploadimg: res.tempFilePaths[0]
          })

        }
      })
     
    }
  },
  editImage: function () {
    this.setData({
      editable: !this.data.editable
    })
  },
  deleteImg: function (e) {
    console.log(e.currentTarget.dataset.index);
    const imgs = this.data.uploadimgs
    // Array.prototype.remove = function(i){
    //   const l = this.length;
    //   if(l==1){
    //     return []
    //   }else if(i>1){
    //     return [].concat(this.splice(0,i),this.splice(i+1,l-1))
    //   }
    // }
    this.setData({
      uploadimgs: imgs.remove(e.currentTarget.dataset.index)
    })
  },
  //提交发布
  formSubmit:function(e)
  {
    //可提前判断已提交，防止重复提交
    wx.showToast({
      title: '正在提交...',
      icon: 'loading',
      mask: true,
      duration: 10000
    })
    var that = this;
    var uploadImgCount = 0;
    var order=1;
    if (order==1)
    {
      //上传室内图片
      for (var i = 0, h = that.data.uploadimgs.length; i < h; i++) {
        wx.uploadFile({
          url: 'http://localhost:8113/api/House/UploadFileNew',
          filePath: that.data.uploadimgs[i],
          name: 'uploadfile_ant',
          formData: {
            'imgIndex': i
          },
          header: {
            "Content-Type": "multipart/form-data"
          },
          success: function (res) {
            uploadImgCount++;
            console.log("cg")
            that.setData({
              imageUrls: that.data.imageUrls += (res.data + ',').replace(/\"/g, "")
            });
            //如果是最后一张,则隐藏等待中  
            if (uploadImgCount == that.data.uploadimgs.length) {
              wx.hideToast();
              //进入下一步
              //order = 2;
              //***************不能这么做，一个方法体内的动作会同一时间内执行，只是当时条件不成立没执行，这里让它条件成立了，但也不会再执行力********************* */
              //所以直接调用即可
              that.wjuploadFile();
            }
          },
          fail:function()
          {
            console.log("shibai")
          }
        })
      }
      
    }
  

  },
  wjuploadFile:function()
  {
   var _this=this;
    //上传外景图片
    wx.uploadFile({
      url: 'http://localhost:8113/api/House/UploadFileNew',
      filePath: _this.data.uploadimg,
      name: 'uploadfile_ant',
      formData: {
        'imgIndex': 0
      },
      header: {
        "Content-Type": "multipart/form-data"
      },
      success: function (res) {
        _this.setData({
          imageUrl: res.data.replace(/\"/g, "")
        });
        _this.addHouse();
      }
    })

  },
  addHouse:function()
  {
    var that=this;
    //提交房屋信息
    wx.getStorage({
      key: 'token',
      success: function (res) {
        wx.request({
          url: 'http://localhost:8113/api/House/AddHouse',
          method: 'get',
          data: {
            UserId: app.globalData.userOpenId,
            HabitableRoom: that.data.habitableRoom[that.data.habitableRoomindex],
            House_Area: that.data.area,
            House_Address: that.data.address,
            HouseLocation: that.data.location,
            HouseFacility: that.data.facility,
            House_OwnerTel: that.data.tel,
            House_RentMoney: that.data.rentMoney,
            ImageUrls: that.data.imageUrls,
            ExteriorImage: that.data.imageUrl,
            roleId: that.data.roleId
          },
          header: {
            'content-type': 'application/json',
            'Authorization': 'BasicAuth ' + res.data
          },
          success: function (res) {
           
            if (res.data == 1) {
              wx.showToast({

                title: '发布成功',

                icon: 'success',

                duration: 2000//持续时间，还可用超市函数退出，如下

              })
              var roleId = app.globalData.roleId;
              var checkIndex = app.globalData.currentData;
              wx.navigateTo({
                url: '/pages/personal/personal?roleId=' + roleId + '&checkIndex=' + checkIndex,
              })
            }
            else if (res.data == 2){
              wx.showToast({

                title: '提交成功，待审批',

                icon: 'success',

                duration: 2000//持续时间，还可用超市函数退出，如下

              })
              var roleId = app.globalData.roleId;
              var checkIndex = app.globalData.currentData;
              wx.navigateTo({
                url: '/pages/personal/personal?roleId=' + roleId + '&checkIndex=' + checkIndex,
              })
            }
            else{
              wx.showToast({

                title: '提交失败',

                icon: 'fail',

                duration: 2000//持续时间，还可用超市函数退出，如下

              })
            }
          

          },
          fail:function()
          {
            wx.showToast({

              title: '提交失败，请完整信息',

              icon: 'fail',

              duration: 2000//持续时间，还可用超市函数退出，如下

            })
          }
        })/**注意括号，有时不报错 */
      }
    })
  },
  adressChange:function(e)
  {
    this.setData({
      address:e.detail.value
    })
  },
  locationChange: function (e) {
    this.setData({
      location: e.detail.value
    })
  },
  areaChange: function (e) {
    this.setData({
      area: e.detail.value
    })
  },
  facilityChange: function (e) {
    this.setData({
      facility: e.detail.value
    })
  },
  rentMoneyChange: function (e) {
    this.setData({
      rentMoney: e.detail.value
    })
  },
  nameChange: function (e) {
    this.setData({
      name: e.detail.value
    })
  },
  telChange: function (e) {
    this.setData({
      tel: e.detail.value
    })
  },
  IDChange: function (e) {
    this.setData({
      ID: e.detail.value
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