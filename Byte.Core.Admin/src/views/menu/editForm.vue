<template>
  <el-dialog
    :title="title"
    v-model="visible"
    width="80%"
    destroy-on-close
    draggable
    overflow
    align-center
    @close="close"
  >
    <el-form ref="ruleFormRef" :model="form" label-width="160px" :rules="rules">
      <el-form-item label="父级菜单" prop="parentId">
        <menu-select v-model="form.parentId" />
      </el-form-item>

      <el-form-item label="菜单名称" prop="title">
        <el-input v-model="form.title" placeholder="请输入菜单名称" />
      </el-form-item>

      <el-form-item label="菜单类型" prop="type">
        <el-radio-group v-model="form.type" @change="onMenuTypeChange">
          <el-radio
            :label="item"
            v-for="(item, index) in getEnumValue(MenuTypeEnum)"
            :key="index"
            >{{ MenuTypeEnum[item] }}</el-radio
          >
        </el-radio-group>
      </el-form-item>

      <el-form-item
        v-if="form.type == MenuTypeEnum.外链"
        label="外链地址"
        prop="path"
      >
        <el-input v-model="form.path" placeholder="请输入外链完整路径" />
      </el-form-item>

      <el-form-item
        v-if="form.type == MenuTypeEnum.目录 || form.type == MenuTypeEnum.菜单"
        label="路由路径"
        prop="path"
      >
        <el-input
          v-if="form.type == MenuTypeEnum.目录"
          v-model="form.path"
          placeholder="system"
        />
        <el-input v-else v-model="form.path" placeholder="user" />
      </el-form-item>

      <!-- 组件页面完整路径 -->
      <el-form-item
        v-if="form.type == MenuTypeEnum.菜单"
        label="页面路径"
        prop="component"
      >
        <el-input
          v-model="form.component"
          placeholder="system/user/index"
          style="width: 95%"
        >
          <template v-if="form.type == MenuTypeEnum.菜单" #prepend
            >src/views/</template
          >
          <template v-if="form.type == MenuTypeEnum.菜单" #append
            >.vue</template
          >
        </el-input>
      </el-form-item>

      <el-form-item
        v-if="form.type !== MenuTypeEnum.按钮"
        prop="hidden"
        label="显示状态"
      >
        <el-radio-group v-model="form.hidden">
          <el-radio :label="false">显示</el-radio>
          <el-radio :label="true">隐藏</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item
        v-if="form.type === MenuTypeEnum.目录"
        label="根目录始终显示"
      >
        <template #label>
          <div>
            根目录始终显示
            <el-tooltip placement="bottom" effect="light">
              <template #content
                >是：根目录只有一个子路由显示目录
                <br />否：根目录只有一个子路由不显示目录，只显示子路由
              </template>
              <i-ep-QuestionFilled class="inline-block" />
            </el-tooltip>
          </div>
        </template>

        <el-radio-group v-model="form.alwaysShow">
          <el-radio :label="true">是</el-radio>
          <el-radio :label="false">否</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item v-if="form.type === MenuTypeEnum.菜单" label="是否缓存">
        <el-radio-group v-model="form.keepAlive">
          <el-radio :label="true">是</el-radio>
          <el-radio :label="false">否</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item
        v-if="form.keepAlive && form.type === MenuTypeEnum.菜单"
        label="组件名称名称"
        prop="componentName"
      >
        <el-input v-model="form.componentName" placeholder="user" />
      </el-form-item>

      <el-form-item label="排序" prop="sort">
        <el-input-number
          v-model="form.sort"
          style="width: 100px"
          controls-position="right"
          :min="0"
        />
      </el-form-item>

      <!-- 权限标识 -->
      <el-form-item
        v-if="form.type == MenuTypeEnum.按钮"
        label="权限标识"
        prop="perm"
      >
        <el-input v-model="form.perm" placeholder="sys:user:add" />
      </el-form-item>

      <el-form-item
        v-if="form.type !== MenuTypeEnum.按钮"
        label="图标"
        prop="icon"
      >
        <!-- 图标选择器 -->
        <icon-select v-model="form.icon" />
      </el-form-item>

      <el-form-item v-if="form.type == MenuTypeEnum.目录" label="跳转路由">
        <el-input v-model="form.redirect" placeholder="跳转路由" />
      </el-form-item>

      <el-form-item label="状态" prop="state">
        <el-switch
          v-model="form.state"
          class="ml-2"
          inline-prompt
          style="--el-switch-on-color: #13ce66; --el-switch-off-color: #ff4949"
          active-text="启用"
          inactive-text="禁用"
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="visible = false">取消</el-button>
        <el-button
          v-hasPerm="['menu/update']"
          type="primary"
          @click="handleSubmit"
          :loading="loading"
        >
          保 存
        </el-button>
      </span>
    </template>
  </el-dialog>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, FormInstance } from "element-plus";
import menuSelect from "@/views/menu/select.vue";
import { MenuTypeEnum, getEnumValue } from "@/api/apiEnums";
import { useRoute } from "vue-router";
import "@/api";
import { useForm } from "alova/client";
import { MenuInfo, MenuTreeDTO, UpdateMenuParam } from "@/api/globals";
const route = useRoute();
const emit = defineEmits(["refresh"]);
/**
 * 获取详情
 */
const { send: getInfo } = useRequest(
  (id) =>
    Apis.Menu.get_api_menu_getinfo({
      params: { id },
      transform: (res) => {
        updateForm(res.data);
      },
    }),
  { immediate: false }
);
/**
 * 提交详情
 */
const {
  send: submit,
  form,
  updateForm,
} = useForm(
  (form) =>
    Apis.Menu.post_api_menu_submit({
      data: form,
      transform: () => {
        visible.value = false;
        ElMessage.success("保存成功！");
        emit("refresh");
      },
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as UpdateMenuParam,
  }
);

const ruleFormRef = ref<FormInstance>();
const visible = ref(false);
const title = ref(route.meta.title);
const loading = ref(false);
const close = () => {
  ruleFormRef.value?.resetFields();
  form.value = {};
};
const rules = reactive({
  // parentId: [{ required: true, message: "请选择顶级菜单", trigger: "blur" }],
  title: [{ required: true, message: "请输入菜单名称", trigger: "blur" }],
  type: [{ required: true, message: "请选择菜单类型", trigger: "blur" }],
  path: [{ required: true, message: "请输入路由路径", trigger: "blur" }],

  component: [{ required: true, message: "请输入组件路径", trigger: "blur" }],
  componentName: [
    { required: true, message: "请输入组件名称", trigger: "blur" },
  ],
  visible: [{ required: true, message: "请输入路由路径", trigger: "blur" }],
});
//打开
const openForm = async (id?: string, parentRow?: MenuTreeDTO) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  } else {
    form.value.parentId = parentRow?.id;
    form.value.state = true;
  }
};

/** 菜单类型切换事件处理 */
function onMenuTypeChange() {
  // 如果菜单类型改变，清空路由路径；未改变在切换后还原路由路径
  if (form.value.type !== MenuTypeEnum.目录) {
    form.value.path = "";
  }
  // else {
  //   form.value.path = menuCacheData.path;
  // }
}

//   clickChild(){

//    }

//保存
async function handleSubmit() {
  const valid: boolean | undefined = await ruleFormRef.value?.validate();
  if (!valid) {
    return;
  }
  await submit(form.value);
}
defineExpose({
  openForm,
});
</script>
