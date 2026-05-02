export function PageLoading() {
  return (
    <div className="flex items-center justify-center min-h-screen text-gray-600 text-lg">
      <span className="ml-2 flex items-end gap-1">
        {/* css styling */}
        <span className="animate-loading dot-1" />
        <span className="animate-loading dot-2" />
        <span className="animate-loading dot-3" />
      </span>
    </div>
  );
}
