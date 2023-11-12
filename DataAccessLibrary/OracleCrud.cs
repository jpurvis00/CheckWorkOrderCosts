
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class OracleCrud
    {
        private readonly string _connectionString;
        private OracleDataAccess _db = new OracleDataAccess();

        public OracleCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClosedWorkOrderModel> GetClosedWorkOrders()
        {
            string sql = "select * from z_closed_jobs_nightly where close_date = TRUNC(SYSDATE - 1)";

            return _db.LoadData<ClosedWorkOrderModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<CostSummaryModel> GetCostSummaryActualMaterialCost(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, total_mat_cost from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'WO_ISSUES'";

            return _db.LoadData<CostSummaryModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<WorkOrderIssuesWCostModel> GetWOIssuesWithCost(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"component_item_no, lot_no, total_mat_cost, total_labor_cost, total_foh_cost, total_sc_cost " +
                $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'WO_ISSUES'";

            return _db.LoadData<WorkOrderIssuesWCostModel, dynamic>(sql, new { }, _connectionString);
        }
        
        public List<TimeCardDetailsModel> GetTimeCardDetails(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"employee_number, operation_code, work_center_no, good_qty, scrap_qty, qa_hold_qty, total_labor_cost, total_foh_cost " +
                $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'RUN_ACTUAL'";

            return _db.LoadData<TimeCardDetailsModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<SubContractingModel> GetSubContractingDetails(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"transaction_qty_buom, total_sc_cost from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'PO_RECEIPT'";

            return _db.LoadData<SubContractingModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<WorkOrderCompletionModel> GetWorkOrderCompletionDetails(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"transaction_qty_buom, lot_no, total_mat_cost, total_labor_cost, total_foh_cost, total_sc_cost " +
                $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'COMPLETION'";

            return _db.LoadData<WorkOrderCompletionModel, dynamic>(sql, new { }, _connectionString);
        }
    }
}
