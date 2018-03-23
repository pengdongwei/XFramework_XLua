// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: SoundData.proto

#define INTERNAL_SUPPRESS_PROTOBUF_FIELD_DEPRECATION
#include "SoundData.pb.h"

#include <algorithm>

#include <google/protobuf/stubs/common.h>
#include <google/protobuf/stubs/once.h>
#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/wire_format_lite_inl.h>
#include <google/protobuf/descriptor.h>
#include <google/protobuf/generated_message_reflection.h>
#include <google/protobuf/reflection_ops.h>
#include <google/protobuf/wire_format.h>
// @@protoc_insertion_point(includes)

namespace dbc {

namespace {

const ::google::protobuf::Descriptor* SoundDataTable_descriptor_ = NULL;
const ::google::protobuf::internal::GeneratedMessageReflection*
  SoundDataTable_reflection_ = NULL;
const ::google::protobuf::Descriptor* SoundData_descriptor_ = NULL;
const ::google::protobuf::internal::GeneratedMessageReflection*
  SoundData_reflection_ = NULL;

}  // namespace


void protobuf_AssignDesc_SoundData_2eproto() {
  protobuf_AddDesc_SoundData_2eproto();
  const ::google::protobuf::FileDescriptor* file =
    ::google::protobuf::DescriptorPool::generated_pool()->FindFileByName(
      "SoundData.proto");
  GOOGLE_CHECK(file != NULL);
  SoundDataTable_descriptor_ = file->message_type(0);
  static const int SoundDataTable_offsets_[2] = {
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundDataTable, tname_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundDataTable, tlist_),
  };
  SoundDataTable_reflection_ =
    new ::google::protobuf::internal::GeneratedMessageReflection(
      SoundDataTable_descriptor_,
      SoundDataTable::default_instance_,
      SoundDataTable_offsets_,
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundDataTable, _has_bits_[0]),
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundDataTable, _unknown_fields_),
      -1,
      ::google::protobuf::DescriptorPool::generated_pool(),
      ::google::protobuf::MessageFactory::generated_factory(),
      sizeof(SoundDataTable));
  SoundData_descriptor_ = file->message_type(1);
  static const int SoundData_offsets_[3] = {
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundData, id_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundData, desc_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundData, path_),
  };
  SoundData_reflection_ =
    new ::google::protobuf::internal::GeneratedMessageReflection(
      SoundData_descriptor_,
      SoundData::default_instance_,
      SoundData_offsets_,
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundData, _has_bits_[0]),
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(SoundData, _unknown_fields_),
      -1,
      ::google::protobuf::DescriptorPool::generated_pool(),
      ::google::protobuf::MessageFactory::generated_factory(),
      sizeof(SoundData));
}

namespace {

GOOGLE_PROTOBUF_DECLARE_ONCE(protobuf_AssignDescriptors_once_);
inline void protobuf_AssignDescriptorsOnce() {
  ::google::protobuf::GoogleOnceInit(&protobuf_AssignDescriptors_once_,
                 &protobuf_AssignDesc_SoundData_2eproto);
}

void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedMessage(
    SoundDataTable_descriptor_, &SoundDataTable::default_instance());
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedMessage(
    SoundData_descriptor_, &SoundData::default_instance());
}

}  // namespace

void protobuf_ShutdownFile_SoundData_2eproto() {
  delete SoundDataTable::default_instance_;
  delete SoundDataTable_reflection_;
  delete SoundData::default_instance_;
  delete SoundData_reflection_;
}

void protobuf_AddDesc_SoundData_2eproto() {
  static bool already_here = false;
  if (already_here) return;
  already_here = true;
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
    "\n\017SoundData.proto\022\003dbc\">\n\016SoundDataTable"
    "\022\r\n\005tname\030\001 \001(\t\022\035\n\005tlist\030\002 \003(\0132\016.dbc.Sou"
    "ndData\"3\n\tSoundData\022\n\n\002ID\030\001 \001(\005\022\014\n\004Desc\030"
    "\002 \001(\t\022\014\n\004path\030\003 \001(\t", 139);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "SoundData.proto", &protobuf_RegisterTypes);
  SoundDataTable::default_instance_ = new SoundDataTable();
  SoundData::default_instance_ = new SoundData();
  SoundDataTable::default_instance_->InitAsDefaultInstance();
  SoundData::default_instance_->InitAsDefaultInstance();
  ::google::protobuf::internal::OnShutdown(&protobuf_ShutdownFile_SoundData_2eproto);
}

// Force AddDescriptors() to be called at static initialization time.
struct StaticDescriptorInitializer_SoundData_2eproto {
  StaticDescriptorInitializer_SoundData_2eproto() {
    protobuf_AddDesc_SoundData_2eproto();
  }
} static_descriptor_initializer_SoundData_2eproto_;

// ===================================================================

#ifndef _MSC_VER
const int SoundDataTable::kTnameFieldNumber;
const int SoundDataTable::kTlistFieldNumber;
#endif  // !_MSC_VER

SoundDataTable::SoundDataTable()
  : ::google::protobuf::Message() {
  SharedCtor();
}

void SoundDataTable::InitAsDefaultInstance() {
}

SoundDataTable::SoundDataTable(const SoundDataTable& from)
  : ::google::protobuf::Message() {
  SharedCtor();
  MergeFrom(from);
}

void SoundDataTable::SharedCtor() {
  _cached_size_ = 0;
  tname_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
}

SoundDataTable::~SoundDataTable() {
  SharedDtor();
}

void SoundDataTable::SharedDtor() {
  if (tname_ != &::google::protobuf::internal::kEmptyString) {
    delete tname_;
  }
  if (this != default_instance_) {
  }
}

void SoundDataTable::SetCachedSize(int size) const {
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
}
const ::google::protobuf::Descriptor* SoundDataTable::descriptor() {
  protobuf_AssignDescriptorsOnce();
  return SoundDataTable_descriptor_;
}

const SoundDataTable& SoundDataTable::default_instance() {
  if (default_instance_ == NULL) protobuf_AddDesc_SoundData_2eproto();
  return *default_instance_;
}

SoundDataTable* SoundDataTable::default_instance_ = NULL;

SoundDataTable* SoundDataTable::New() const {
  return new SoundDataTable;
}

void SoundDataTable::Clear() {
  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    if (has_tname()) {
      if (tname_ != &::google::protobuf::internal::kEmptyString) {
        tname_->clear();
      }
    }
  }
  tlist_.Clear();
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
  mutable_unknown_fields()->Clear();
}

bool SoundDataTable::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!(EXPRESSION)) return false
  ::google::protobuf::uint32 tag;
  while ((tag = input->ReadTag()) != 0) {
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // optional string tname = 1;
      case 1: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->mutable_tname()));
          ::google::protobuf::internal::WireFormat::VerifyUTF8String(
            this->tname().data(), this->tname().length(),
            ::google::protobuf::internal::WireFormat::PARSE);
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(18)) goto parse_tlist;
        break;
      }

      // repeated .dbc.SoundData tlist = 2;
      case 2: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED) {
         parse_tlist:
          DO_(::google::protobuf::internal::WireFormatLite::ReadMessageNoVirtual(
                input, add_tlist()));
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(18)) goto parse_tlist;
        if (input->ExpectAtEnd()) return true;
        break;
      }

      default: {
      handle_uninterpreted:
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_END_GROUP) {
          return true;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, mutable_unknown_fields()));
        break;
      }
    }
  }
  return true;
#undef DO_
}

void SoundDataTable::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // optional string tname = 1;
  if (has_tname()) {
    ::google::protobuf::internal::WireFormat::VerifyUTF8String(
      this->tname().data(), this->tname().length(),
      ::google::protobuf::internal::WireFormat::SERIALIZE);
    ::google::protobuf::internal::WireFormatLite::WriteString(
      1, this->tname(), output);
  }

  // repeated .dbc.SoundData tlist = 2;
  for (int i = 0; i < this->tlist_size(); i++) {
    ::google::protobuf::internal::WireFormatLite::WriteMessageMaybeToArray(
      2, this->tlist(i), output);
  }

  if (!unknown_fields().empty()) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        unknown_fields(), output);
  }
}

::google::protobuf::uint8* SoundDataTable::SerializeWithCachedSizesToArray(
    ::google::protobuf::uint8* target) const {
  // optional string tname = 1;
  if (has_tname()) {
    ::google::protobuf::internal::WireFormat::VerifyUTF8String(
      this->tname().data(), this->tname().length(),
      ::google::protobuf::internal::WireFormat::SERIALIZE);
    target =
      ::google::protobuf::internal::WireFormatLite::WriteStringToArray(
        1, this->tname(), target);
  }

  // repeated .dbc.SoundData tlist = 2;
  for (int i = 0; i < this->tlist_size(); i++) {
    target = ::google::protobuf::internal::WireFormatLite::
      WriteMessageNoVirtualToArray(
        2, this->tlist(i), target);
  }

  if (!unknown_fields().empty()) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        unknown_fields(), target);
  }
  return target;
}

int SoundDataTable::ByteSize() const {
  int total_size = 0;

  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    // optional string tname = 1;
    if (has_tname()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::StringSize(
          this->tname());
    }

  }
  // repeated .dbc.SoundData tlist = 2;
  total_size += 1 * this->tlist_size();
  for (int i = 0; i < this->tlist_size(); i++) {
    total_size +=
      ::google::protobuf::internal::WireFormatLite::MessageSizeNoVirtual(
        this->tlist(i));
  }

  if (!unknown_fields().empty()) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        unknown_fields());
  }
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = total_size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
  return total_size;
}

void SoundDataTable::MergeFrom(const ::google::protobuf::Message& from) {
  GOOGLE_CHECK_NE(&from, this);
  const SoundDataTable* source =
    ::google::protobuf::internal::dynamic_cast_if_available<const SoundDataTable*>(
      &from);
  if (source == NULL) {
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
    MergeFrom(*source);
  }
}

void SoundDataTable::MergeFrom(const SoundDataTable& from) {
  GOOGLE_CHECK_NE(&from, this);
  tlist_.MergeFrom(from.tlist_);
  if (from._has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    if (from.has_tname()) {
      set_tname(from.tname());
    }
  }
  mutable_unknown_fields()->MergeFrom(from.unknown_fields());
}

void SoundDataTable::CopyFrom(const ::google::protobuf::Message& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void SoundDataTable::CopyFrom(const SoundDataTable& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool SoundDataTable::IsInitialized() const {

  return true;
}

void SoundDataTable::Swap(SoundDataTable* other) {
  if (other != this) {
    std::swap(tname_, other->tname_);
    tlist_.Swap(&other->tlist_);
    std::swap(_has_bits_[0], other->_has_bits_[0]);
    _unknown_fields_.Swap(&other->_unknown_fields_);
    std::swap(_cached_size_, other->_cached_size_);
  }
}

::google::protobuf::Metadata SoundDataTable::GetMetadata() const {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::Metadata metadata;
  metadata.descriptor = SoundDataTable_descriptor_;
  metadata.reflection = SoundDataTable_reflection_;
  return metadata;
}


// ===================================================================

#ifndef _MSC_VER
const int SoundData::kIDFieldNumber;
const int SoundData::kDescFieldNumber;
const int SoundData::kPathFieldNumber;
#endif  // !_MSC_VER

SoundData::SoundData()
  : ::google::protobuf::Message() {
  SharedCtor();
}

void SoundData::InitAsDefaultInstance() {
}

SoundData::SoundData(const SoundData& from)
  : ::google::protobuf::Message() {
  SharedCtor();
  MergeFrom(from);
}

void SoundData::SharedCtor() {
  _cached_size_ = 0;
  id_ = 0;
  desc_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  path_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
}

SoundData::~SoundData() {
  SharedDtor();
}

void SoundData::SharedDtor() {
  if (desc_ != &::google::protobuf::internal::kEmptyString) {
    delete desc_;
  }
  if (path_ != &::google::protobuf::internal::kEmptyString) {
    delete path_;
  }
  if (this != default_instance_) {
  }
}

void SoundData::SetCachedSize(int size) const {
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
}
const ::google::protobuf::Descriptor* SoundData::descriptor() {
  protobuf_AssignDescriptorsOnce();
  return SoundData_descriptor_;
}

const SoundData& SoundData::default_instance() {
  if (default_instance_ == NULL) protobuf_AddDesc_SoundData_2eproto();
  return *default_instance_;
}

SoundData* SoundData::default_instance_ = NULL;

SoundData* SoundData::New() const {
  return new SoundData;
}

void SoundData::Clear() {
  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    id_ = 0;
    if (has_desc()) {
      if (desc_ != &::google::protobuf::internal::kEmptyString) {
        desc_->clear();
      }
    }
    if (has_path()) {
      if (path_ != &::google::protobuf::internal::kEmptyString) {
        path_->clear();
      }
    }
  }
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
  mutable_unknown_fields()->Clear();
}

bool SoundData::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!(EXPRESSION)) return false
  ::google::protobuf::uint32 tag;
  while ((tag = input->ReadTag()) != 0) {
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // optional int32 ID = 1;
      case 1: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::int32, ::google::protobuf::internal::WireFormatLite::TYPE_INT32>(
                 input, &id_)));
          set_has_id();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(18)) goto parse_Desc;
        break;
      }

      // optional string Desc = 2;
      case 2: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED) {
         parse_Desc:
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->mutable_desc()));
          ::google::protobuf::internal::WireFormat::VerifyUTF8String(
            this->desc().data(), this->desc().length(),
            ::google::protobuf::internal::WireFormat::PARSE);
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(26)) goto parse_path;
        break;
      }

      // optional string path = 3;
      case 3: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED) {
         parse_path:
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->mutable_path()));
          ::google::protobuf::internal::WireFormat::VerifyUTF8String(
            this->path().data(), this->path().length(),
            ::google::protobuf::internal::WireFormat::PARSE);
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectAtEnd()) return true;
        break;
      }

      default: {
      handle_uninterpreted:
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_END_GROUP) {
          return true;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, mutable_unknown_fields()));
        break;
      }
    }
  }
  return true;
#undef DO_
}

void SoundData::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // optional int32 ID = 1;
  if (has_id()) {
    ::google::protobuf::internal::WireFormatLite::WriteInt32(1, this->id(), output);
  }

  // optional string Desc = 2;
  if (has_desc()) {
    ::google::protobuf::internal::WireFormat::VerifyUTF8String(
      this->desc().data(), this->desc().length(),
      ::google::protobuf::internal::WireFormat::SERIALIZE);
    ::google::protobuf::internal::WireFormatLite::WriteString(
      2, this->desc(), output);
  }

  // optional string path = 3;
  if (has_path()) {
    ::google::protobuf::internal::WireFormat::VerifyUTF8String(
      this->path().data(), this->path().length(),
      ::google::protobuf::internal::WireFormat::SERIALIZE);
    ::google::protobuf::internal::WireFormatLite::WriteString(
      3, this->path(), output);
  }

  if (!unknown_fields().empty()) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        unknown_fields(), output);
  }
}

::google::protobuf::uint8* SoundData::SerializeWithCachedSizesToArray(
    ::google::protobuf::uint8* target) const {
  // optional int32 ID = 1;
  if (has_id()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteInt32ToArray(1, this->id(), target);
  }

  // optional string Desc = 2;
  if (has_desc()) {
    ::google::protobuf::internal::WireFormat::VerifyUTF8String(
      this->desc().data(), this->desc().length(),
      ::google::protobuf::internal::WireFormat::SERIALIZE);
    target =
      ::google::protobuf::internal::WireFormatLite::WriteStringToArray(
        2, this->desc(), target);
  }

  // optional string path = 3;
  if (has_path()) {
    ::google::protobuf::internal::WireFormat::VerifyUTF8String(
      this->path().data(), this->path().length(),
      ::google::protobuf::internal::WireFormat::SERIALIZE);
    target =
      ::google::protobuf::internal::WireFormatLite::WriteStringToArray(
        3, this->path(), target);
  }

  if (!unknown_fields().empty()) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        unknown_fields(), target);
  }
  return target;
}

int SoundData::ByteSize() const {
  int total_size = 0;

  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    // optional int32 ID = 1;
    if (has_id()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::Int32Size(
          this->id());
    }

    // optional string Desc = 2;
    if (has_desc()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::StringSize(
          this->desc());
    }

    // optional string path = 3;
    if (has_path()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::StringSize(
          this->path());
    }

  }
  if (!unknown_fields().empty()) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        unknown_fields());
  }
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = total_size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
  return total_size;
}

void SoundData::MergeFrom(const ::google::protobuf::Message& from) {
  GOOGLE_CHECK_NE(&from, this);
  const SoundData* source =
    ::google::protobuf::internal::dynamic_cast_if_available<const SoundData*>(
      &from);
  if (source == NULL) {
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
    MergeFrom(*source);
  }
}

void SoundData::MergeFrom(const SoundData& from) {
  GOOGLE_CHECK_NE(&from, this);
  if (from._has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    if (from.has_id()) {
      set_id(from.id());
    }
    if (from.has_desc()) {
      set_desc(from.desc());
    }
    if (from.has_path()) {
      set_path(from.path());
    }
  }
  mutable_unknown_fields()->MergeFrom(from.unknown_fields());
}

void SoundData::CopyFrom(const ::google::protobuf::Message& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void SoundData::CopyFrom(const SoundData& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool SoundData::IsInitialized() const {

  return true;
}

void SoundData::Swap(SoundData* other) {
  if (other != this) {
    std::swap(id_, other->id_);
    std::swap(desc_, other->desc_);
    std::swap(path_, other->path_);
    std::swap(_has_bits_[0], other->_has_bits_[0]);
    _unknown_fields_.Swap(&other->_unknown_fields_);
    std::swap(_cached_size_, other->_cached_size_);
  }
}

::google::protobuf::Metadata SoundData::GetMetadata() const {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::Metadata metadata;
  metadata.descriptor = SoundData_descriptor_;
  metadata.reflection = SoundData_reflection_;
  return metadata;
}


// @@protoc_insertion_point(namespace_scope)

}  // namespace dbc

// @@protoc_insertion_point(global_scope)