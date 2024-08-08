<template>
  <el-dialog
    :title="title"
    v-model="visible"
    width="80%"
    top="12vh"
    destroy-on-close
    @close="close"
  >
    <el-form :model="form" ref="ruleFormRef" label-width="100px">
      <el-row>
        <el-col :span="12">
          <el-form-item label="上级组织" prop="parentId">
            <dept-select v-model="form.parentId" />
          </el-form-item>
          <el-form-item
            label="类型"
            prop="type"
            :rules="{
              required: true,
              message: '类型不能为空',
              trigger: ['blur', 'change'],
            }"
          >
            <el-select v-model="form.type" placeholder="请选择类型">
              <el-option
                :key="index"
                :label="DeptTypeEnum[item]"
                :value="item"
                v-for="(item, index) in getEnumValue(DeptTypeEnum)"
              />
            </el-select>
          </el-form-item>

          <el-form-item
            label="组织名称"
            prop="name"
            :rules="{
              required: true,
              message: '组织名称不能为空',
              trigger: ['blur', 'change'],
            }"
          >
            <el-input
              v-model="form.name"
              autocomplete="off"
              placeholder="请输入组织名称"
            />
          </el-form-item>
          <el-form-item label="组织简称" prop="easyName">
            <el-input
              v-model="form.easyName"
              autocomplete="off"
              placeholder="请输入组织简称"
            />
          </el-form-item>

          <el-form-item label="地址" prop="address">
            <el-input
              v-model="form.address"
              autocomplete="off"
              placeholder="请输入地址"
            />
          </el-form-item>

          <el-form-item label="状态" prop="state">
            <el-switch
              v-model="form.state"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
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
          <el-form-item label="备注" prop="remark">
            <el-input
              v-model="form.remark"
              type="textarea"
              rows="3"
              autocomplete="off"
              placeholder="请输入备注"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="Logo" prop="image">
            <AvatarUpLoad v-model="form.image" />
          </el-form-item>
          <el-form-item label="法人/负责人" prop="man">
            <el-input
              v-model="form.man"
              autocomplete="off"
              placeholder="请输入法人/负责人"
            />
          </el-form-item>
          <el-form-item label="法人扫码">
            <div>请前往测试靶场</div>
          </el-form-item>

          <el-form-item label="联系电话" prop="phone">
            <el-input
              v-model="form.phone"
              autocomplete="off"
              placeholder="请输入联系电话"
            />
          </el-form-item>
          <el-form-item label="验证码" prop="phone">
            <el-input
              v-model="form.msgCode"
              autocomplete="off"
              placeholder="请输入验证码"
            />
            <div>请前往测试靶场</div>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <template #footer>
      <el-button @click="visible = false">取消</el-button>

      <el-button
        v-hasPerm="['dept/update']"
        type="primary"
        @click="handleSubmit"
        :loading="loading"
      >
        保 存
      </el-button>
    </template>
  </el-dialog>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, FormInstance } from "element-plus";
import { DeptTypeEnum, getEnumValue } from "@/api/apiEnums";
import deptSelect from "@/views/dept/select.vue";
import AvatarUpLoad from "@/components/Upload/AvatarUpLoad.vue";
import { useRoute } from "vue-router";
import "@/api";
import { useForm } from "alova/client";
import { DeptTreeDTO, UpdateDeptParam } from "@/api/globals";
const route = useRoute();
const emit = defineEmits(["refresh"]);
/**
 * 获取详情
 */
const { send: getInfo } = useRequest(
  (id) =>
    Apis.Dept.get_api_dept_getinfo({
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
  reset: resetFrom,
  updateForm,
} = useForm(
  (form) =>
    Apis.Dept.post_api_dept_submit({
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
    initialForm: {
      parentId: "",
      msgCode: 123456,
    } as UpdateDeptParam,
  }
);

const ruleFormRef = ref<FormInstance>();
const visible = ref(false);
const title = ref(route.meta.title);
const loading = ref(false);
const close = () => {
  ruleFormRef.value?.resetFields();
  resetFrom();
};
//打开
const openForm = async (id?: string, parentRow?: DeptTreeDTO) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  } else {
    form.value.parentId = parentRow?.id;
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
  form,
});
</script>
