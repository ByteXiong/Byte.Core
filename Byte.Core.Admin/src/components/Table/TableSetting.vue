<template>
  <el-drawer title="列设置" v-model="visible" size="90%">
    <el-form>
      <el-form-item
        label="表格描述"
        prop="title"
        :rules="{
          required: true,
          message: '表格不能为空',
          trigger: ['blur', 'change'],
        }"
      >
        <el-input v-model="form.title" placeholder="请输入表格" />
      </el-form-item>

      <el-form-item label="表字段" prop="title">
        <VueDraggable
          target="tbody"
          v-model="form.tableColumns"
          :animation="150"
        >
          <el-table
            :data="form?.tableColumns"
            v-loading="loading"
            highlightCurrentRow
            rowKey="id"
          >
            <el-tableColumn label="拖拽" width="80">
              <template #default>
                <div class="drag-table">
                  <Setting style="width: 2em; height: 2em; margin-right: 8px" />
                </div>
              </template>
            </el-tableColumn>
            <el-tableColumn label="列名" prop="label" width="120">
              <template #default="{ row }">
                <el-input
                  v-model="row.label"
                  placeholder="请输入列名"
                ></el-input>
              </template>
            </el-tableColumn>

            <el-tableColumn label="字段名" prop="prop" width="120">
              <!-- <template #default="{ row }">
              <el-input
                v-model="row.prop"
                placeholder="请输入字段名"
              ></el-input>
            </template> -->
            </el-tableColumn>

            <el-tableColumn label="是否显示" prop="isShow" width="80">
              <template #default="{ row }">
                <el-switch v-model="row.isShow"></el-switch>
              </template>
            </el-tableColumn>
            <el-tableColumn label="排序" prop="sortable" width="80">
              <template #default="{ row }">
                <el-switch v-model="row.sortable"></el-switch>
              </template>
            </el-tableColumn>

            <el-tableColumn label="宽度" prop="width">
              <template #default="{ row }">
                <el-inputNumber
                  controls
                  v-model="row.width"
                  placeholder="请输入宽度"
                ></el-inputNumber>
              </template>
            </el-tableColumn>
            <el-tableColumn label="是否排序" prop="sort">
              <template #default="{ row }">
                <el-inputNumber
                  controls
                  v-model="row.sort"
                  placeholder="请输入排序"
                ></el-inputNumber>
              </template>
            </el-tableColumn>

            <el-tableColumn label="搜索条件" prop="condition">
              <template #default="{ row }">
                <el-select v-model="row.condition">
                  <el-option label="自定义" value="-1"></el-option>
                  <el-option label="无" value="0"></el-option>
                  <el-option label="等于" value="1"></el-option>
                  <el-option label="不等于" value="2"></el-option>
                  <el-option label="大于" value="3"></el-option>
                </el-select>
              </template>
            </el-tableColumn>

            <el-tableColumn label="插槽重写" prop="condition">
              <template #default="{ row }">
                <el-tag>head</el-tag>
                <el-tag>default</el-tag>
              </template>
            </el-tableColumn>
          </el-table>
        </VueDraggable>
      </el-form-item>
    </el-form>
    <template #footer>
      <div>
        <el-button @click="restore">还原</el-button>
        <el-button type="primary" @click="submit"> 确定 </el-button>
      </div>
    </template>
  </el-drawer>
</template>
<script setup lang="ts">
import { TableModelInfo, UpdateTableModelParam } from "@/api/globals";
import { CheckboxValueType, ElTable } from "element-plus";
import { VueDraggable } from "vue-draggable-plus";
const { tableof } = defineProps<{
  tableof: string;
}>();
const visible = defineModel<boolean>();
// 获取数据
const { loading } = useRequest(
  () =>
    Apis.TableModel.get_api_tablemodel_getinfo({
      params: {
        Table: tableof,
        Router: "/demo",
      },
      transform: (res) => {
        form.value = res.data;
        emit("tableData", form.value);
      },
    }),
  {
    debounce: 500,
    immediate: true,
    async middleware(_, next) {
      if (tableof) {
        await next();
      } else {
        console.error("tableColumn组件 请配置tableof 返回模型DTO");
      }
    },
  }
);

/**
 * 提交详情
 */
const { send: submit, form } = useForm(
  (form) =>
    Apis.TableModel.post_api_tablemodel_submit({
      data: form,
      transform: () => {
        visible.value = false;
        emit("tableData", form);
        ElMessage.success("保存成功！");
      },
    }),
  {
    immediate: false,
    resetAfterSubmiting: false,
    initialForm: {} as UpdateTableModelParam,
    async middleware(_, next) {
      form.value.router = "/demo";
      await next();
    },
  }
);
const isIndeterminate = ref(true);
const emit = defineEmits(["tableData"]);
console.log(ElTable);

const confirm = () => {
  visible.value = false;
};
const onStart = () => {
  console.log("start");
};
const onEnd = () => {
  console.log("end");
};
const restore = () => {};
defineExpose({ visible });
</script>
