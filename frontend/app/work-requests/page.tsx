'use client';

import { useState, useEffect, useCallback } from 'react';
import { WorkRequestSummary, WorkRequestStatus } from '@/types/workRequest';
import { workRequestApi } from '@/services/api';
import WorkRequestFilters from '@/components/work-requests/WorkRequestFilters';
import WorkRequestTable from '@/components/work-requests/WorkRequestTable';
import CreateRequestModal from '@/components/work-requests/CreateRequestModal';
import UpdateStatusModal from '@/components/work-requests/UpdateStatusModal';

export default function WorkRequestsPage() {
  // State
  const [workRequests, setWorkRequests] = useState<WorkRequestSummary[]>([]);
  const [filters, setFilters] = useState({
    status: '',
    search: '',
    page: 1,
    pageSize: 10,
  });
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [selectedIdForStatus, setSelectedIdForStatus] = useState<string | null>(null);

  const totalPages = Math.max(1, Math.ceil(totalCount / filters.pageSize));

  // Data fetching
  const fetchWorkRequests = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const result = await workRequestApi.getAll(filters);
      setWorkRequests(result.data);
      setTotalCount(result.pagination?.totalCount ?? 0);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Something went wrong.');
    } finally {
      setIsLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchWorkRequests();
  }, [fetchWorkRequests]);

  // Event handlers
  const handleFilterChange = (newFilters: { status: string; search: string }) => {
    setFilters({ ...newFilters, page: 1, pageSize: filters.pageSize });
  };

  const handleStatusUpdate = (id: string) => {
    setSelectedIdForStatus(id);
  };

  const handlePrevPage = () => {
    if (filters.page > 1) {
      setFilters({ ...filters, page: filters.page - 1 });
    }
  };

  const handleNextPage = () => {
    if (filters.page < totalPages) {
      setFilters({ ...filters, page: filters.page + 1 });
    }
  };

  // Find current status for the selected work request
  const selectedWorkRequest = workRequests.find((wr) => wr.id === selectedIdForStatus);


  return (
    <div className="min-h-screen bg-gray-950 text-gray-100">
      <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="mb-6">
          <h1 className="text-2xl font-bold text-white">Work Requests</h1>
          <p className="mt-1 text-sm text-gray-400">
            Manage and track all client work requests
          </p>
        </div>

        {/* Filters */}
        <div className="mb-6">
          <WorkRequestFilters
            filters={{ status: filters.status, search: filters.search }}
            onFilterChange={handleFilterChange}
            onOpenCreate={() => setIsCreateModalOpen(true)}
          />
        </div>

        {/* Error banner */}
        {error && (
          <div className="mb-4 rounded-lg border border-red-500/30 bg-red-500/10 px-4 py-3 text-sm text-red-400">
            {error}
          </div>
        )}

        {/* Table */}
        <WorkRequestTable
          workRequests={workRequests}
          isLoading={isLoading}
          onStatusUpdate={handleStatusUpdate}
        />

        {/* Pagination */}
        {!isLoading && totalCount > 0 && (
          <div className="mt-4 flex items-center justify-between">
            <p className="text-sm text-gray-400">
              {totalCount} total result{totalCount !== 1 ? 's' : ''}
            </p>
            <div className="flex items-center gap-3">
              <button
                onClick={handlePrevPage}
                disabled={filters.page <= 1}
                className="rounded-lg border border-gray-700 px-3 py-2 text-sm text-gray-300 transition-colors hover:border-gray-600 hover:text-white disabled:cursor-not-allowed disabled:opacity-40"
              >
                Previous
              </button>
              <span className="text-sm text-gray-400">
                Page {filters.page} of {totalPages}
              </span>
              <button
                onClick={handleNextPage}
                disabled={filters.page >= totalPages}
                className="rounded-lg border border-gray-700 px-3 py-2 text-sm text-gray-300 transition-colors hover:border-gray-600 hover:text-white disabled:cursor-not-allowed disabled:opacity-40"
              >
                Next
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Create Modal */}
      <CreateRequestModal
        isOpen={isCreateModalOpen}
        onClose={() => setIsCreateModalOpen(false)}
        onCreated={fetchWorkRequests}
      />

      {/* Update Status Modal */}
      <UpdateStatusModal
        isOpen={selectedIdForStatus !== null}
        workRequestId={selectedIdForStatus ?? ''}
        currentStatus={selectedWorkRequest?.status ?? 'New'}
        onClose={() => setSelectedIdForStatus(null)}
        onUpdated={fetchWorkRequests}
      />
    </div>
  );
}
