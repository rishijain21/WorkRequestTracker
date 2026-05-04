'use client';

import { useState, useEffect, useRef } from 'react';

interface WorkRequestFiltersProps {
  filters: { status: string; search: string };
  onFilterChange: (filters: { status: string; search: string }) => void;
  onOpenCreate: () => void;
}

export default function WorkRequestFilters({
  filters,
  onFilterChange,
  onOpenCreate,
}: WorkRequestFiltersProps) {
  const [searchInput, setSearchInput] = useState(filters.search);
  const debounceTimer = useRef<NodeJS.Timeout | null>(null);

  // Debounce search — wait 400ms after user stops typing
  useEffect(() => {
    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }

    debounceTimer.current = setTimeout(() => {
      if (searchInput !== filters.search) {
        onFilterChange({ ...filters, search: searchInput });
      }
    }, 400);

    return () => {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }
    };
  }, [searchInput]);

  const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    onFilterChange({ ...filters, status: e.target.value });
  };

  return (
    <div className="flex items-center gap-3">
      {/* Search input — grows to fill available space */}
      <div className="relative flex-1">
        <svg
          className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
          />
        </svg>
        <input
          type="text"
          placeholder="Search by title or client..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          className="w-full rounded-lg border border-gray-700 bg-gray-800 py-2.5 pl-10 pr-4 text-sm text-gray-100 placeholder-gray-500 outline-none transition-colors focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500"
        />
      </div>

      {/* Status dropdown */}
      <select
        value={filters.status}
        onChange={handleStatusChange}
        className="rounded-lg border border-gray-700 bg-gray-800 px-4 py-2.5 text-sm text-gray-100 outline-none transition-colors focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500"
      >
        <option value="">All Statuses</option>
        <option value="New">New</option>
        <option value="InProgress">In Progress</option>
        <option value="Blocked">Blocked</option>
        <option value="Completed">Completed</option>
      </select>

      {/* + New Request button */}
      <button
        onClick={onOpenCreate}
        className="flex items-center gap-2 whitespace-nowrap rounded-lg bg-indigo-600 px-5 py-2.5 text-sm font-medium text-white transition-colors hover:bg-indigo-500 active:bg-indigo-700"
      >
        <svg className="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
        </svg>
        New Request
      </button>
    </div>
  );
}
