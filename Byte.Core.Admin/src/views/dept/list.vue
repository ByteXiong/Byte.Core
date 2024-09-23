<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <div class="flex justify-between mb-2">
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" :content="'新增'" placement="top">
            <!-- <el-button type="primary"  icon="Plus"  circle @click="openForm()" >  </el-button> -->
            <el-icon @click="openForm()"><Plus /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="批量删除" placement="top">
            <el-icon @click="handleDelete(selectIds)"><Delete /></el-icon>
          </el-tooltip>

          <!-- <el-button type="danger" :disabled="selectIds.length==0"  @click="handleDelete(selectIds)">批量删除</el-button> -->
        </div>
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" content="刷新" placement="top">
            <el-icon @click="getTree"><Refresh /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="导出" placement="top">
            <el-icon><Download /></el-icon>
          </el-tooltip>

          <!-- <el-tooltip effect="light" content="设置表头" placement="top">
            <el-icon><Setting /></el-icon>
          </el-tooltip> -->
        </div>
      </div>
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

        <el-table-column show-overflow-tooltip prop="name" label="组织名称" />

        <el-table-column
          show-overflow-tooltip
          prop="easyName"
          label="组织简称"
          width="100"
        />

        <el-table-column show-overflow-tooltip label="Logo" width="80">
          <template #default="scope">
            <el-avatar shape="square" :src="upuloadUrl + scope.row.image" />
          </template>
        </el-table-column>

        <el-table-column show-overflow-tooltip label="类型" width="80">
          <template #default="scope">
            <el-tag :type="DeptTypeEl[scope.row.type]">
              {{ DeptTypeEnum[scope.row.type] }}</el-tag
            >
          </template>
        </el-table-column>

        <el-table-column
          show-overflow-tooltip
          prop="man"
          label="法人/负责人"
          width="100"
        />
        <el-table-column
          show-overflow-tooltip
          prop="phone"
          label="联系电话"
          width="130"
        />
        <el-table-column show-overflow-tooltip prop="address" label="地址" />
        <el-table-column show-overflow-tooltip prop="remark" label="备注" />
        <el-table-column
          show-overflow-tooltip
          prop="sort"
          label="排序"
          width="80"
        />

        <el-table-column show-overflow-tooltip label="状态" width="90">
          <template #default="scope">
            <el-switch
              v-model="scope.row.state"
              :loading="stateLoading"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
              @change="
                async (e) => {
                  setState(scope.row.id, e);
                  // getData();
                }
              "
            />
          </template>
        </el-table-column>
        <el-table-column
          show-overflow-tooltip
          class-name="onExcel"
          label="操作"
          width="280"
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
              v-hasPerm="['dept/update']"
              type="primary"
              link
              size="small"
              @click.stop="openChildForm(scope.row)"
              ><i-ep-plus />组织架构</el-button
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
      <edit-form ref="editform" @refresh="getTree" />
    </el-card>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import editForm from "./editForm.vue";
import { DeptTypeEnum } from "@/api/apiEnums";
import { DeptTypeEl } from "@/api/apiEls";
import "@/api";
import { DeptTreeDTO, RoleDTO } from "@/api/globals";
defineOptions({
  name: "Dept",
  inheritAttrs: false,
});
const upuloadUrl = import.meta.env.VITE_UPLOAD_PROXY_PREFIX;
const keyWord = ref("");
/**
 * 获取数据
 */
const {
  data,
  loading,
  send: getTree,
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () => Apis.Dept.get_api_dept_gettree({ transform: ({ data }) => data }),
  {
    force: true,
    immediate: true,
  }
);
/**
 * 删除
 */
const { send: delIds } = useRequest(
  (ids: string[]) =>
    Apis.Dept.delete_api_dept_delete({
      data: ids,
      transform: () => {
        ElMessage.success("删除成功");
        getTree();
      },
    }),
  { immediate: false }
);
/**
 * 设置状态
 */
const { send: setState, loading: stateLoading } = useRequest(
  (id: string, state: boolean) =>
    Apis.Dept.put_api_dept_setstate({
      params: { id, state },
      transform: (res) => {
        ElMessage.success("状态更新成功");
        return res.data;
      },
    }),
  {
    immediate: false,
  }
);

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
const openForm = async (id?: string) => {
  await editform.value.openForm(id);
};

const openChildForm = async (row: DeptTreeDTO) => {
  await editform.value.openForm(null, row);
};
</script>
