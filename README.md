# 卡牌-组件化游戏框架 架构说明文档

## 1. 框架概述

本框架是一个基于Unity的卡牌-组件化游戏架构，采用**消息驱动**和**组件化设计**，实现了高度解耦的游戏系统。框架核心思想是将游戏中的所有实体抽象为"卡牌"(Card)，通过系统(System)进行逻辑处理，使用消息机制进行通信。

## 2. 核心架构层级

### 2.1 数据层 (Data Layer)
- **DataManager**: 数据管理中心，负责加载和管理所有卡牌数据、配置和资源
- **CardData**: 卡牌元数据，包含ID、名称、描述、配置等
- **MetaData**: 字段元数据，定义卡牌的分类和资源路径
- **DataBase**: 配置数据的基类，可通过反射动态初始化

### 2.2 实体层 (Entity Layer)
- **CardBase**: 所有游戏实体的基类
  - 包含ID、父节点、组件列表等基础属性
  - 支持序列化（使用JsonIgnore标记运行时数据）
  - 采用树形结构组织组件关系

- **几个比较特殊的卡牌类型**:
  - `CObj_2`: 游戏世界中的实体对象，绑定GameObject
  - `Clive_19`: 生命实体
  - `Citem_33`: 物品实体

### 2.3 系统层 (System Layer)
- **SystemManager**: 系统管理器，核心的消息分发和处理中心
  - 管理所有系统(SystemBase)的初始化和生命周期
  - 提供消息发送机制，支持优先级处理
  - 处理卡牌的创建、克隆、描述解析

- **SystemBase**: 系统基类
  - 每个卡牌类型对应一个系统
  - 注册消息处理函数(Handler)
  - 实现卡牌的创建、克隆、描述解析等逻辑

### 2.4 管理层 (Manager Layer)
- **CardManager**: 卡牌管理器
  - 卡牌的创建、销毁、复制
  - 组件关系的管理（添加/移除/激活/反激活）
  - 卡牌与GameObject的绑定

- **World**: 游戏世界管理器
  - Unity场景入口点
  - 初始化所有管理器
  - 协程支持

## 3. 关键设计模式

### 3.1 组件化模式
- **父子关系**: 卡牌通过`parent`和`components`形成树形结构
- **激活组件**: `activeComponents`用于动态添加/移除组件效果
- **组件查找**: 提供类型安全的组件查询接口

### 3.2 消息驱动模式
- **MsgType**: 定义所有消息类型，包含100+种游戏事件
- **消息分类**:
  - Self消息：只发送给单个卡牌
  - AddToObj消息：需要进入对象队列的消息
  - 普通消息：广播给组件树的所有节点
- **优先级系统**: 支持优先级等级，确保处理顺序

### 3.3 反射与动态类型
- 动态创建卡牌：`Type.GetType()` + `Activator.CreateInstance()`
- 反射获取字段值：支持卡牌字段和配置数据的动态访问
- 字符串模板解析：`{变量名}`和`[变量名]`的动态替换

## 4. 核心流程

### 4.1 初始化流程
```
World.Awake()
  ├─ DataManager.Init()     # 加载所有卡牌数据
  ├─ SystemManager.Init()   # 初始化所有系统
  └─ CardManager.Init()     # 初始化卡牌管理器
```

### 4.2 卡牌创建流程
```
CardManager.CreateCard()
  ├─ Activator.CreateInstance()  # 反射创建实例
  ├─ SystemManager.GetCardId()   # 获取类型ID
  ├─ SystemManager.TriCreateCard() # 系统处理创建逻辑
  └─ 设置UID和其他基础属性
```

### 4.3 消息处理流程
```
SystemManager.SendMsg()
  ├─ 获取处理列表（考虑组件树和优先级）
  ├─ 排序处理函数（按优先级和exPriority）
  ├─ 遍历执行处理函数
  └─ 支持消息有效性检查和中途退出
```

### 4.4 组件管理流程
```
CardManager.AddComponent()
  ├─ 建立父子关系
  ├─ ActiveComponent()激活组件
  ├─ AddHandlerToCobj()注册消息处理器
  └─ 发送OnItem消息通知组件状态变化
```

## 5. 数据结构设计

### 5.1 卡牌ID体系
- **PID**: 物理ID，全局唯一
- **VID**: 虚拟ID，字段内唯一
- **UID**: 运行时唯一标识符
- 通过`fieldId`映射表实现字段划分

### 5.2 消息处理器存储
```csharp
// 三级存储结构
SystemBase.handler           # 基础处理器列表
CObj_2.msgTypeToHandler      # 对象级处理器缓存（优化性能）
CardAndPriority              # 运行时处理器包装（包含生命周期）
```

### 5.3 资源路径解析
```
Resources/[数据路径]/[字段文件夹][字段名]/[ID范围]/[VID]_[原始名称]/[资源文件]
示例: Resources/0_data/card/0~9/1_MyCard/myPrefab.prefab
```

## 6. 特性与优化

### 6.1 性能优化
- **处理器缓存**: CObj_2对象缓存消息处理器，减少遍历
- **生命周期管理**: 通过`liveCnt`检测过期的处理器
- **玩家优化**: 特定事件只发送给主玩家

### 6.2 扩展性
- **字符串模板**: 支持动态变量替换和颜色标记
- **反射系统**: 无需硬编码即可访问卡牌属性
- **组件热插拔**: 运行时动态添加/移除组件

### 6.3 调试支持
- 消息类型调试显示
- 组件关系验证
- 递归深度保护

## 7. 使用示例

### 7.1 创建游戏对象
```csharp
// 创建对象并且放到世界里
CLplayer_1 player = CardManager.CreateCard<CLplayer_1>();
AddToWorld(player);
```

### 7.2 发送消息
```csharp
// 发送碰撞消息
MsgCollision msg = new MsgCollision(attacker, target, collision.gameObject, hitPoint);
SystemManager.SendMsg(target, MsgType.Collision, msg);
```

### 7.3 添加组件
```csharp
// 给玩家添加地图组件
CCmap_45 map = CardManager.CreateCard<CCmap_45>();
CardManager.AddComponent(player, map);
```

## 8. 注意事项

1. **循环引用**: 注意卡牌父子关系中的循环引用问题
2. **消息顺序**: 优先级相同的处理器执行顺序可能不固定
3. **性能敏感**: 反射操作和字符串解析可能影响性能

---

这个框架通过高度抽象和灵活的消息机制，为复杂的卡牌类游戏提供了强大的基础架构，同时保持了良好的扩展性和可维护性（大概？）。
逻辑已经完备，可以造出一个正常的游戏，可见已有卡牌示例