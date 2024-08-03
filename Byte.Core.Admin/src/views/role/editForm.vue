<template>
  <el-dialog
    :title="title"
    v-model="visible"
    width="80%"
    destroy-on-close
    @close="close"
  >
    <el-form :model="form" ref="ruleFormRef" label-width="100px">
      <el-form-item
        label="名称"
        prop="name"
        :rules="{
          required: true,
          message: '名称不能为空',
          trigger: ['blur', 'change'],
        }"
      >
        <el-input
          v-model="form.name"
          autocomplete="off"
          placeholder="请输入名称"
        />
      </el-form-item>
      <el-form-item
        label="编码"
        prop="code"
        :rules="{
          required: true,
          message: '编码不能为空',
          trigger: ['blur', 'change'],
        }"
      >
        <el-input
          v-model="form.code"
          autocomplete="off"
          placeholder="请输入编码"
        />
      </el-form-item>
      <el-form-item label="排序" prop="sort">
        <el-input
          type="number"
          v-model="form.sort"
          autocomplete="off"
          placeholder="请输入排序"
        />
      </el-form-item>
      <el-form-item
        label="类型"
        prop="type"
        :rules="{
          required: true,
          message: '类型不能为空',
          trigger: ['blur', 'change'],
        }"
        ><el-select
          filterable
          v-model="form.type"
          placeholder="选择类型"
          style="width: 240px"
        >
          <el-option
            v-for="item in getEnumValue(RoleTypeEnum)"
            :key="item"
            :label="RoleTypeEnum[item]"
            :value="item"
        /></el-select>
      </el-form-item>

      <el-form-item label="备注" prop="remark">
        <el-input
          type="textarea"
          rows="3"
          v-model="form.remark"
          autocomplete="off"
          placeholder="请输入备注"
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="visible = false">取消</el-button>
        <el-button
          v-hasPerm="['role/add']"
          type="primary"
          @click="handleSubmit"
          :loading="loading"
        >
          新 增
        </el-button>
        <el-button
          v-hasPerm="['role/update']"
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
import { RoleTypeEnum, getEnumValue } from "@/api/apiEnums";
import { useRoute } from "vue-router";
import "@/api";
import { useForm } from "alova/client";
import { UpdateRoleParam } from "@/api/globals";
const route = useRoute();
const emit = defineEmits(["refresh"]);
/**
 * 获取详情
 */
const { send: getInfo } = useRequest(
  (id) =>
    Apis.Role.get_api_role_getinfo({
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
    Apis.Role.post_api_role_submit({
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
    initialForm: {} as UpdateRoleParam,
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

//打开
const openForm = async (id?: string) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  } else {
    form.value.state = true;
  }
};
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
