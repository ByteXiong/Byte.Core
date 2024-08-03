<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <template #header>
        <div class="flex justify-between">
          <div>
            <el-input
              style="width: 150px; margin: 0px 5px"
              v-model="keyWord"
              placeholder="关键字查询"
              clearable
              @keyup.enter="search"
            />

            <el-button :loading="loading" type="primary" @click="search"
              ><i-ep-search />搜索</el-button
            >
            <!-- <el-button :loading="loading" @click=""
              ><i-ep-refresh />重置</el-button
            > -->
            <el-button
              :loading="loading"
              v-hasPerm="['dept/add']"
              type="success"
              @click="openForm()"
              ><i-ep-plus />新增</el-button
            >
            <el-button
              :loading="loading"
              v-hasPerm="['dept/update']"
              type="primary"
              :disabled="selectIds.length === 0"
              @click="openForm(selectIds[selectIds.length - 1])"
              ><i-ep-edit />编辑</el-button
            >
            <el-button
              :loading="loading"
              v-hasPerm="['dept/delete']"
              type="danger"
              :disabled="selectIds.length === 0"
              @click="handleDelete(selectIds)"
              ><i-ep-delete />删除</el-button
            >
          </div>
        </div>
      </template>
      <el-table
        v-loading="loading"
        highlight-current-row
        row-key="id"
        :data="data"
        :border="true"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        @selection-change="handleSelectionChange"
        @row-dblclick="
          (row: DeptTreeDTO) => {
            openForm(row.id);
          }
        "
        ref="tableRef"
      >
        <el-table-column
          show-overflow-tooltip
          type="selection"
          width="55"
          class-name="onExcel"
        />
        <el-table-column type="index" width="50px" />
        <el-table-column show-overflow-tooltip prop="name" label="单位名称" />
        <el-table-column show-overflow-tooltip prop="address" label="地址" />
        <el-table-column
          show-overflow-tooltip
          prop="easyName"
          label="单位简称"
          width="100"
        />
        <el-table-column
          show-overflow-tooltip
          prop="man"
          label="备用联系人"
          width="100"
        />
        <el-table-column
          show-overflow-tooltip
          prop="phone"
          label="联系电话"
          width="130"
        />

        <el-table-column show-overflow-tooltip prop="remark" label="备注" />
        <el-table-column
          show-overflow-tooltip
          prop="sort"
          label="排序"
          width="80"
        />

        <el-table-column
          show-overflow-tooltip
          prop="state"
          label="状态"
          width="80"
          fixed="right"
        >
          <template #default="scope">
            <el-tag
              :type="scope.row.state ? 'success' : 'danger'"
              effect="dark"
            >
              {{ scope.row.state ? "启用" : "禁用" }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column
          show-overflow-tooltip
          class-name="onExcel"
          label="操作"
          width="160"
          fixed="right"
        >
          <template #default="scope">
            <el-button
              :loading="loading"
              v-hasPerm="['dept/update']"
              type="primary"
              link
              size="small"
              @click.stop="openForm(scope.row.id)"
              ><i-ep-edit />编辑</el-button
            >
            <el-button
              :loading="loading"
              v-hasPerm="['dept/delete']"
              type="primary"
              link
              size="small"
              @click.stop="handleDelete([scope.row.id])"
              ><i-ep-delete />删除</el-button
            >
          </template>
        </el-table-column>

        <template #empty>
          <el-empty description="暂无数据" />
        </template>
      </el-table>
      <!-- <template #footer>
        <div class="padding-t10 text-align-right">
          <el-pagination
            small
            background
            layout="total, sizes, prev, pager, next, jumper"
            v-model:currentPage="pagerInfo.pageIndex"
            :page-sizes="[10, 50, 100]"
            :page-size="pagerInfo.pageSize"
            :total="pagerInfo.totalRowCount"
            @size-change="sizeChenge"
            @current-change="pageChange"
          />
        </div>
      </template> -->
      <edit-form ref="editform" @refresh="getData" />
      <upload ref="uploadRef" @refresh="getData" />
    </el-card>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import editForm from "./editForm.vue";
import { RoleTypeEnum } from "@/api/apiEnums";
import "@/api";
import { DeptTreeDTO, RoleDTO } from "@/api/globals";
defineOptions({
  name: "Dept",
  inheritAttrs: false,
});
const keyWord = ref("");
/**
 * 获取数据
 */
const {
  data,
  loading,
  send: getData,
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () => Apis.Dept.get_api_dept_gettree({ transform: ({ data }) => data }),
  {
    immediate: true,
  }
);
/**
 * 删除
 */
const { send: delIds } = useRequest(
  (ids: string[]) => Apis.Dept.delete_api_dept_delete({ data: ids }),
  { immediate: false }
);
/**
 * 设置状态
 */
const { send: setState } = useRequest(
  (id: string, state: boolean) =>
    Apis.Dept.put_api_dept_setstate({ params: { id, state } }),
  {
    immediate: false,
  }
);

async function search() {
  await getData();
}

const selectIds = ref<string[]>([]);
//多选
async function handleSelectionChange(e: any) {
  selectIds.value = e.map((x: any) => x.id);
}

// 删除
async function handleDelete(ids: string[]) {
  if (ids.length == 0) {
    ElMessage.error("请勾选批量删除条目");
    return;
  }
  ElMessageBox.confirm("确定删除吗？", "删除", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await delIds(ids);
    })
    .catch((error: any) => {
      console.log(error);
    });
}
//打开子页面
const editform = ref();
async function openForm(id?: string) {
  await editform.value.openForm(id);
}
</script>
