

版本更新6.20.03
1.设置slider不能手动拖拽
2.背包里面的血量/蓝条/亲密字体显示准确值
3.战斗画面添加等级显示

版本更新6.20.02
1.给已有排序的beast设置为battle则为清除排序，要给出战顺序重新排序

版本更新6.20.01
1.如果点了battle键，将弹出一个下拉panel，内含button #1, #2, #3,#4，#5，#6，改变兽出战顺序。 背包中的前6只兽按出战顺序排序
  a)做一个dropdown监听，当下拉菜单选择时，传回数值选择，然后直接改变背包内beasts的排序。并且更新最新显示
  b)如果目前只有#1，其他排序都为null，自动assign到#2
  c)如果target的位置有东西，把原来的东西往后挪
      <1, 2, 3, 4>   <- (Null -> 1)
      <1(新的东西), 2（原1）, 3（原2）, 4（原3）, 5（原4）>
  d)当原有出战顺序和目标出战顺序都不为空时，
      4->1 ， 原来的1->2, 2->3, 3->4按顺序往后挪
      1->4， 出战顺序互换
  e)当把有出战排序的beast变为无出战排序时，把后面的出战排序自动前移

版本更新6.19.02
1.修复地图右边的beast只跳跳不走路，有时候重生在空地方的问题。现在beast会随机平均生在已圈地图的所有范围内。

版本更新6.19.01 
1.战斗结束后地图上的beast消失（直接没加到我的dont destroy里）
2.战斗结束以后把beast放回地图上
3.Maintown和Countryside之间的地图转换并不会影响previous的保存
4.beast的刷新和恢复搞定（解决clone删不掉的情况）

版本更新6.16.07
1.合成成功面板显示/合成成功后在背包中删除辅助兽
2.血条蓝条颜色变更
3.做了detailpanel的出战顺序UI，但是还没改变出战顺序

版本更新6.16.06
合成成功面板显示/合成成功后在背包中删除辅助兽并且更新主宠数值

版本更新6.16.05
1.直接从start里生成初始宠，不从respawn里拿 
2.融合成功界面panel
3.传送碑和合成碑又亮了。Graphic里的z = 1不能改
4.新加入三个beast ：小鸡，小猫，小骷髅

版本更新6.16.04
1.Normal宠数值生成不能超过紫色， 先生成再决定type 
2.删除beastInfoUI的脚本
3.直接从start里生成初始宠，不从respawn里拿 1/2

版本更新6.16.03
1.战斗结束后返回上一层
2.返回上一层后怪不会重新刷新 生成时记录了兽的id
3.Normal宠数值生成不能超过紫色， 先生成再决定type 

版本更新6.16.02
1.战斗界面对beast资料的更新
2.先判断先手，然后轮着打
3.战斗界面的显示和血条/蓝条动态update
3.1血条panel上的详细数值显示

6.14
1.修改beast bag panel的显示框
2.BattleScene框架完成
3.碰到beast后转换场景完成
