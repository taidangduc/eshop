import { useRef, useState } from "react";

export function ImageUploader({
  size = 80,
  maxImages = null,
  showText = false,
  text = "Add image",
  onChange,
}) {
  const [images, setImages] = useState([]);
  const fileInputRef = useRef(null);

  const handleClick = () => {
    fileInputRef.current?.click();
  };

  const handleFileChange = (event) => {
    const files = Array.from(event.target.files);

    if (maxImages !== null && images.length + files.length > maxImages) {
      alert(`Maximum ${maxImages} image(s) allowed`);
      return;
    }

    const newImages = files.map((file) => ({
      file,
      preview: URL.createObjectURL(file),
    }));

    const updatedImages = [...images, ...newImages];
    setImages(updatedImages);

    if (onChange) {
      onChange(updatedImages.map((img) => img.file));
    }
  };

  const handleRemoveImage = (index, e) => {
    e.stopPropagation();
    URL.revokeObjectURL(images[index].preview);
    const updatedImages = images.filter((_, i) => i !== index);
    setImages(updatedImages);

    if (onChange) {
      onChange(updatedImages.map((img) => img.file));
    }
  };

  return (
    <div className="flex flex-wrap gap-2">
      {/* Display uploaded images */}
      {images.map((image, index) => (
        <div
          key={index}
          className="relative border rounded-[2px] group"
          style={{ width: size, height: size }}
        >
          <img
            src={image.preview}
            alt={`Upload ${index + 1}`}
            className="w-full h-full object-cover rounded-[2px]"
          />
          {/* Overlay with fade effect */}
          <div className="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-50 transition-all duration-300 rounded-[2px] flex items-center justify-center opacity-0 group-hover:opacity-100">
            <button
              onClick={(e) => handleRemoveImage(index, e)}
              className="rounded-full w-8 h-8 flex items-center justify-center font-bold"
              style={{
                backgroundColor: "rgb(255, 66, 79)",
                color: "#fff",
                border: "none",
                cursor: "pointer",
                fontSize: "20px",
              }}
            >
              &times;
            </button>
          </div>
        </div>
      ))}

      {/* Upload button (only show if maxImages not reached) */}
      {(maxImages === null || images.length < maxImages) && (
        <div
          className="flex flex-col gap-1 items-center justify-center border border-dashed rounded-[2px] cursor-pointer hover:bg-gray-50"
          style={{ width: size, height: size }}
          onClick={handleClick}
        >
          <div>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width={24}
              height={24}
              viewBox="0 0 24 24"
            >
              <path
                fill="#000"
                d="M21 15v3h3v2h-3v3h-2v-3h-3v-2h3v-3zm.008-12c.548 0 .992.445.992.993v9.349A6 6 0 0 0 20 13V5H4l.001 14l9.292-9.293a1 1 0 0 1 1.32-.084l.094.085l3.545 3.55a6.003 6.003 0 0 0-3.91 7.743L2.992 21A.993.993 0 0 1 2 20.007V3.993A1 1 0 0 1 2.992 3zM8 7a2 2 0 1 1 0 4a2 2 0 0 1 0-4"
              ></path>
            </svg>
          </div>
          {showText && (
            <div className="text-[12px] text-center">
              {text} {maxImages !== null && `(${images.length}/${maxImages})`}
            </div>
          )}
        </div>
      )}

      {/* Hidden file input */}
      <input
        ref={fileInputRef}
        type="file"
        accept="image/*"
        multiple={maxImages === null || maxImages > 1}
        onChange={handleFileChange}
        style={{ display: "none" }}
      />
    </div>
  );
}
